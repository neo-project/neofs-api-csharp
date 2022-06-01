using System;
using Google.Protobuf;
using Neo.FileStorage.API.Cryptography;

namespace Neo.FileStorage.API.Refs
{
    public partial class OwnerID
    {
        public const int ValueSize = 25;
        public const byte AddressVersion = 0x35;

        public static OwnerID FromByteArray(byte[] bytes)
        {
            if (bytes.Length != ValueSize) throw new System.FormatException("invalid owner bytes");
            return new OwnerID
            {
                Value = ByteString.CopyFrom(bytes)
            };
        }

        public static OwnerID FromScriptHash(byte[] scriptHash)
        {
            Span<byte> data = stackalloc byte[21];
            data[0] = AddressVersion;
            scriptHash.CopyTo(data[1..]);
            byte[] checksum = data.ToArray().Sha256().Sha256();
            Span<byte> value = stackalloc byte[data.Length + 4];
            data.CopyTo(value);
            checksum.AsSpan(..4).CopyTo(value[data.Length..]);
            return new()
            {
                Value = ByteString.CopyFrom(value),
            };
        }

        public static OwnerID FromAddress(string address)
        {
            var bytes = Base58.Decode(address);
            return new OwnerID
            {
                Value = ByteString.CopyFrom(bytes),
            };
        }

        public static OwnerID FromPublicKey(byte[] publicKey)
        {
            var bytes = Base58.Decode(publicKey.PublicKeyToAddress());
            return new OwnerID
            {
                Value = ByteString.CopyFrom(bytes),
            };
        }

        public string ToAddress()
        {
            return Base58.Encode(Value.ToByteArray());
        }

        public byte[] ToScriptHash()
        {
            return Value.ToByteArray()[1..^4];
        }
    }
}
