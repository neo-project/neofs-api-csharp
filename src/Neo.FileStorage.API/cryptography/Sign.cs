using System;
using System.Security.Cryptography;
using Google.Protobuf;
using Neo.FileStorage.API.Refs;
using Neo.FileStorage.API.Session;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;

namespace Neo.FileStorage.API.Cryptography
{
    public static class SignExtension
    {
        public const int RFC6979SignatureSize = 64;

        public static byte[] SignRFC6979(this ECDsa key, byte[] data)
        {
            var digest = new Sha256Digest();
            var secp256r1 = SecNamedCurves.GetByName("secp256r1");
            var ec_parameters = new ECDomainParameters(secp256r1.Curve, secp256r1.G, secp256r1.N);
            var private_key = new ECPrivateKeyParameters(new BigInteger(1, key.PrivateKey()), ec_parameters);
            var signer = new ECDsaSigner(new HMacDsaKCalculator(digest));
            var hash = new byte[digest.GetDigestSize()];
            digest.BlockUpdate(data, 0, data.Length);
            digest.DoFinal(hash, 0);
            signer.Init(true, private_key);
            var rs = signer.GenerateSignature(hash);
            var signature = new byte[RFC6979SignatureSize];
            var rbytes = rs[0].ToByteArrayUnsigned();
            var sbytes = rs[1].ToByteArrayUnsigned();
            var index = RFC6979SignatureSize / 2 - rbytes.Length;
            rbytes.CopyTo(signature, index);
            index = RFC6979SignatureSize - sbytes.Length;
            sbytes.CopyTo(signature, index);
            return signature;
        }

        public static Signature SignRFC6979(this ECDsa key, IMessage message)
        {
            return new Signature
            {
                Key = ByteString.CopyFrom(key.PublicKey()),
                Sign = ByteString.CopyFrom(key.SignRFC6979(message.ToByteArray())),
            };
        }

        public static byte[] SignData(this ECDsa key, byte[] data)
        {
            var hash = new byte[65];
            hash[0] = 0x04;
            key
                .SignHash(SHA512.Create().ComputeHash(data))
                .CopyTo(hash, 1);
            return hash;
        }

        public static Signature SignMessagePart(this ECDsa key, IMessage data)
        {
            var data2sign = data is null ? Array.Empty<byte>() : data.ToByteArray();
            var sig = new Signature
            {
                Key = ByteString.CopyFrom(key.PublicKey()),
                Sign = ByteString.CopyFrom(key.SignData(data2sign)),
            };
            return sig;
        }

        public static void Sign(this ECDsa key, IVerificableMessage message)
        {
            var meta = message.GetMetaHeader();
            IVerificationHeader verify = message is IRequest ?
                new RequestVerificationHeader() : message is IResponse ?
                    new ResponseVerificationHeader() : throw new InvalidOperationException("unsupport message type");
            var verifyOrigin = message.GetVerificationHeader();
            if (verifyOrigin is null)
                verify.BodySignature = key.SignMessagePart(message.GetBody());
            verify.MetaSignature = key.SignMessagePart(meta);
            verify.OriginSignature = key.SignMessagePart(verifyOrigin);
            verify.SetOrigin(verifyOrigin);
            message.SetVerificationHeader(verify);
        }

        private static BigInteger[] DecodeSignature(byte[] sig)
        {
            if (sig.Length != RFC6979SignatureSize) throw new FormatException($"Wrong signature size, expect={RFC6979SignatureSize}, actual={sig.Length}");
            var rs = new BigInteger[2];
            rs[0] = new BigInteger(1, sig[..32]);
            rs[1] = new BigInteger(1, sig[32..]);
            return rs;
        }

        public static bool VerifyRFC6979(this byte[] public_key, byte[] data, byte[] sig)
        {
            if (public_key is null || data is null || sig is null) return false;
            var rs = DecodeSignature(sig);
            var digest = new Sha256Digest();
            var signer = new ECDsaSigner(new HMacDsaKCalculator(digest));
            var secp256r1 = SecNamedCurves.GetByName("secp256r1");
            var ec_parameters = new ECDomainParameters(secp256r1.Curve, secp256r1.G, secp256r1.N);
            var bc_public_key = new ECPublicKeyParameters(secp256r1.Curve.DecodePoint(public_key), ec_parameters);
            var hash = new byte[digest.GetDigestSize()];
            digest.BlockUpdate(data, 0, data.Length);
            digest.DoFinal(hash, 0);
            signer.Init(false, bc_public_key);
            return signer.VerifySignature(hash, rs[0], rs[1]);
        }

        public static bool VerifyRFC6979(this Signature signature, IMessage message)
        {
            return signature.Key.ToByteArray().VerifyRFC6979(message.ToByteArray(), signature.Sign.ToByteArray());
        }

        public static bool VerifyData(this ECDsa key, byte[] data, byte[] sig)
        {
            return key.VerifyHash(SHA512.Create().ComputeHash(data), sig[1..]);
        }

        public static bool VerifyMessagePart(this Signature sig, IMessage data)
        {
            if (sig is null || sig.Key is null || sig.Sign is null) return false;
            using var key = sig.Key.ToByteArray().LoadPublicKey();
            var data2verify = data is null ? Array.Empty<byte>() : data.ToByteArray();
            return key.VerifyData(data2verify, sig.Sign.ToByteArray());
        }

        public static bool VerifyMatryoskaLevel(IMessage body, IMetaHeader meta, IVerificationHeader verification)
        {
            if (!verification.MetaSignature.VerifyMessagePart(meta)) return false;
            var origin = verification.GetOrigin();
            if (!verification.OriginSignature.VerifyMessagePart(origin)) return false;
            if (origin is null)
                return verification.BodySignature.VerifyMessagePart(body);
            if (verification.BodySignature is not null) return false;
            return VerifyMatryoskaLevel(body, meta.GetOrigin(), origin);
        }

        public static bool Verify(this IVerificableMessage message)
        {
            return VerifyMatryoskaLevel(message.GetBody(), message.GetMetaHeader(), message.GetVerificationHeader());
        }
    }
}
