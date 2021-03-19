using Google.Protobuf;
using Neo.FileStorage.API.Refs;
using Neo.Cryptography;
using System.Buffers.Binary;
using Neo.FileStorage.API.Cryptography.Tz;

namespace Neo.FileStorage.API.Cryptography
{
    public static class Helper
    {
        public const int Sha256HashLength = 32;

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
            using Murmur3_128 murmur = new(seed);
            return BinaryPrimitives.ReadUInt64LittleEndian(murmur.ComputeHash(value));
        }
    }
}