// Copyright (C) 2015-2025 The Neo Project.
//
// Helper.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Google.Protobuf;
using Neo.FileStorage.API.Cryptography.Tz;
using Neo.FileStorage.API.Refs;
using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Buffers.Binary;
using System.Security.Cryptography;

namespace Neo.FileStorage.API.Cryptography
{
    public static class Helper
    {
        public const int Sha256HashLength = 32;

        internal static byte[] RIPEMD160(this byte[] value)
        {
            var hash = new byte[20];
            var digest = new RipeMD160Digest();
            digest.BlockUpdate(value, 0, value.Length);
            digest.DoFinal(hash, 0);
            return hash;
        }

        public static byte[] Sha256(this byte[] value)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(value);
        }

        internal static byte[] Sha256(this byte[] value, int offset, int count)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(value, offset, count);
        }

        internal static byte[] Sha256(this ReadOnlySpan<byte> value)
        {
            var buffer = new byte[32];
            using var sha256 = SHA256.Create();
            sha256.TryComputeHash(value, buffer, out _);
            return buffer;
        }

        public static ByteString Sha256(this IMessage data)
        {
            return ByteString.CopyFrom(data.ToByteArray().Sha256());
        }

        public static ByteString Sha256(this ByteString data)
        {
            return ByteString.CopyFrom(data.ToByteArray().Sha256());
        }

        public static Checksum Sha256Checksum(this IMessage data)
        {
            return new Checksum
            {
                Type = ChecksumType.Sha256,
                Sum = data.Sha256()
            };
        }

        public static Checksum Sha256Checksum(this ByteString data)
        {
            return new Checksum
            {
                Type = ChecksumType.Sha256,
                Sum = data.Sha256()
            };
        }

        public static ByteString TzHash(this IMessage data)
        {
            return ByteString.CopyFrom(new TzHash().ComputeHash(data.ToByteArray()));
        }

        public static ByteString TzHash(this ByteString data)
        {
            return ByteString.CopyFrom(new TzHash().ComputeHash(data.ToByteArray()));
        }

        public static Checksum TzChecksum(this IMessage data)
        {
            return new Checksum
            {
                Type = ChecksumType.Sha256,
                Sum = data.TzHash()
            };
        }

        public static Checksum TzChecksum(this ByteString data)
        {
            return new Checksum
            {
                Type = ChecksumType.Sha256,
                Sum = data.TzHash()
            };
        }

        public static ulong Murmur64(this byte[] value, uint seed)
        {
            using var murmur = new Murmur3_128(seed);
            return BinaryPrimitives.ReadUInt64LittleEndian(murmur.ComputeHash(value));
        }
    }
}
