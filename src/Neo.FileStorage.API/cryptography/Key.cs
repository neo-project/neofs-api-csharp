using System;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using Google.Protobuf;
using Neo.Cryptography;
using Neo.SmartContract;
using Neo.Wallets;

namespace Neo.FileStorage.API.Cryptography
{
    public static class KeyExtension
    {
        private const int CompressedPublicKeyLength = 33;
        private const int UncompressedPublicKeyLength = 65;

        private static byte[] Decompress(this byte[] public_key)
        {
            if (public_key.Length != CompressedPublicKeyLength)
                throw new FormatException($"{nameof(Decompress)} argument isn't compressed public key. expected length={CompressedPublicKeyLength}, actual={public_key.Length}");
            var point = Neo.Cryptography.ECC.ECPoint.DecodePoint(public_key, Neo.Cryptography.ECC.ECCurve.Secp256r1);
            return point.EncodePoint(false);
        }

        private static byte[] GetPrivateKeyFromWIF(string wif)
        {
            if (wif == null) throw new ArgumentNullException(nameof(wif));
            byte[] data = wif.Base58CheckDecode();
            if (data.Length != 34 || data[0] != 0x80 || data[33] != 0x01)
                throw new FormatException();
            byte[] privateKey = new byte[32];
            Buffer.BlockCopy(data, 1, privateKey, 0, privateKey.Length);
            Array.Clear(data, 0, data.Length);
            return privateKey;
        }

        public static UInt160 PublicKeyToScriptHash(this byte[] public_key)
        {
            var point = Neo.Cryptography.ECC.ECPoint.DecodePoint(public_key, Neo.Cryptography.ECC.ECCurve.Secp256r1);
            return Contract.CreateSignatureRedeemScript(point).ToScriptHash();
        }

        public static byte[] PublicKey(this ECDsa key)
        {
            var param = key.ExportParameters(false);
            var pubkey = new byte[33];
            var pos = 33 - param.Q.X.Length;

            param.Q.X.CopyTo(pubkey, pos);
            if (new BigInteger(param.Q.Y.Reverse().Concat(new byte[] { 0x00 }).ToArray()).IsEven)
            {
                pubkey[0] = 0x2;
            }
            else
            {
                pubkey[0] = 0x3;
            }

            return pubkey;
        }

        public static byte[] PrivateKey(this ECDsa key)
        {
            return key.ExportParameters(true).D;
        }

        public static ECDsa LoadPrivateKey(this byte[] private_key)
        {
            var kp = new KeyPair(private_key);
            var public_key = kp.PublicKey.EncodePoint(false)[1..];
            var key = ECDsa.Create(new ECParameters
            {
                Curve = ECCurve.NamedCurves.nistP256,
                D = private_key,
                Q = new ECPoint
                {
                    X = public_key[..32],
                    Y = public_key[32..]
                }
            });
            return key;
        }

        public static ECDsa LoadWif(this string wif)
        {
            var private_key = GetPrivateKeyFromWIF(wif);
            return LoadPrivateKey(private_key);
        }

        public static ECDsa LoadPublicKey(this ByteString public_key)
        {
            return public_key.ToByteArray().LoadPublicKey();
        }

        public static ECDsa LoadPublicKey(this byte[] public_key)
        {
            if (public_key.Length == CompressedPublicKeyLength)
                public_key = public_key.Decompress();
            if (public_key.Length != UncompressedPublicKeyLength)
                throw new FormatException($"{nameof(LoadPublicKey)} argument isn't public key");
            var public_key_full = public_key[1..];
            var key = ECDsa.Create(new ECParameters
            {
                Curve = ECCurve.NamedCurves.nistP256,
                Q = new ECPoint
                {
                    X = public_key_full[..32],
                    Y = public_key_full[32..]
                }
            });
            return key;
        }
    }
}
