// Copyright (C) 2015-2025 The Neo Project.
//
// Key.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.FileStorage.API.Refs;
using Org.BouncyCastle.Asn1.Sec;
using System;
using System.Buffers.Binary;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using static Neo.FileStorage.API.Helper;

namespace Neo.FileStorage.API.Cryptography;

public static class KeyExtension
{
    public const byte NeoAddressVersion = 0x35;
    private const int CompressedPublicKeyLength = 33;
    private const int UncompressedPublicKeyLength = 65;

    private static readonly uint CheckSigDescriptor =
        BinaryPrimitives.ReadUInt32LittleEndian(Encoding.ASCII.GetBytes("System.Crypto.CheckSig").Sha256());

    public static byte[] Compress(this byte[] publicKey)
    {
        if (publicKey.Length != UncompressedPublicKeyLength)
            throw new FormatException(
                $"{nameof(Compress)} argument isn't uncompressed public key. expected length={UncompressedPublicKeyLength}, actual={publicKey.Length}");
        var secp256r1 = SecNamedCurves.GetByName("secp256r1");
        var point = secp256r1.Curve.DecodePoint(publicKey);
        return point.GetEncoded(true);
    }

    public static byte[] Decompress(this byte[] publicKey)
    {
        if (publicKey.Length != CompressedPublicKeyLength)
            throw new FormatException(
                $"{nameof(Decompress)} argument isn't compressed public key. expected length={CompressedPublicKeyLength}, actual={publicKey.Length}");
        var secp256r1 = SecNamedCurves.GetByName("secp256r1");
        var point = secp256r1.Curve.DecodePoint(publicKey);
        return point.GetEncoded(false);
    }

    private static byte[] CreateSignatureRedeemScript(this byte[] publicKey)
    {
        if (publicKey.Length != CompressedPublicKeyLength)
            throw new FormatException(
                $"{nameof(CreateSignatureRedeemScript)} argument isn't compressed public key. expected length={CompressedPublicKeyLength}, actual={publicKey.Length}");
        var script = new byte[] { 0x0c, CompressedPublicKeyLength }; //PUSHDATA1 33
        script = Concat(script, publicKey);
        script = Concat(script, new byte[] { 0x41 }); //SYSCALL
        script = Concat(script, BitConverter.GetBytes(CheckSigDescriptor)); //Neo_Crypto_CheckSig
        return script;
    }

    public static byte[] GetScriptHash(this byte[] publicKey)
    {
        var script = publicKey.CreateSignatureRedeemScript();
        return script.Sha256().RIPEMD160();
    }

    private static string ToAddress(this byte[] scriptHash, byte version)
    {
        Span<byte> data = stackalloc byte[21];
        data[0] = version;
        scriptHash.CopyTo(data[1..]);
        return Base58.Base58CheckEncode(data);
    }

    private static byte[] GetPrivateKeyFromWIF(string wif)
    {
        if (wif == null) throw new ArgumentNullException();
        var data = wif.Base58CheckDecode();
        if (data.Length != 34 || data[0] != 0x80 || data[33] != 0x01)
            throw new FormatException();
        var privateKey = new byte[32];
        Buffer.BlockCopy(data, 1, privateKey, 0, privateKey.Length);
        Array.Clear(data, 0, data.Length);
        return privateKey;
    }

    public static string Address(this ECDsa key)
    {
        return key.PublicKey().PublicKeyToAddress();
    }

    public static OwnerID OwnerID(this ECDsa key)
    {
        return Refs.OwnerID.FromPublicKey(key.PublicKey());
    }

    public static string PublicKeyToAddress(this byte[] publicKey)
    {
        if (publicKey.Length != CompressedPublicKeyLength)
            throw new FormatException(nameof(publicKey) +
                                      $" isn't encoded compressed public key. expected length={CompressedPublicKeyLength}, actual={publicKey.Length}");
        return publicKey.GetScriptHash().ToAddress(NeoAddressVersion);
    }

    public static byte[] PublicKey(this ECDsa key)
    {
        var param = key.ExportParameters(false);
        var pubkey = new byte[33];
        var pos = 33 - param.Q.X.Length;

        param.Q.X.CopyTo(pubkey, pos);
        if (new BigInteger(param.Q.Y.Reverse().Concat(new byte[] { 0x00 }).ToArray()).IsEven)
            pubkey[0] = 0x2;
        else
            pubkey[0] = 0x3;

        return pubkey;
    }

    public static byte[] PrivateKey(this ECDsa key)
    {
        return key.ExportParameters(true).D;
    }

    public static ECDsa LoadPrivateKey(this byte[] private_key)
    {
        var secp256r1 = SecNamedCurves.GetByName("secp256r1");
        var public_key =
            secp256r1.G.Multiply(new Org.BouncyCastle.Math.BigInteger(1, private_key)).GetEncoded(false)[1..];
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

    public static ECDsa LoadPublicKey(this byte[] public_key)
    {
        var public_key_full = public_key.Decompress()[1..];
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