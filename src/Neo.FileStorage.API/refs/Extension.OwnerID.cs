using System;
using Akka.Util.Internal;
using Google.Protobuf;
using Neo.Cryptography;
using Neo.IO;
using Neo.IO.Json;

namespace Neo.FileStorage.API.Refs
{
    public partial class OwnerID
    {
        public const int ValueSize = 25;

        public static OwnerID FromByteArray(byte[] bytes)
        {
            if (bytes.Length != ValueSize) throw new System.FormatException("invalid owner bytes");
            return new OwnerID
            {
                Value = ByteString.CopyFrom(bytes)
            };
        }

        public static OwnerID FromScriptHash(UInt160 scriptHash)
        {
            Span<byte> data = stackalloc byte[21];
            data[0] = ProtocolSettings.Default.AddressVersion;
            scriptHash.ToArray().CopyTo(data[1..]);
            byte[] checksum = data.Sha256().Sha256();
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

        public string ToAddress()
        {
            return Base58.Encode(Value.ToByteArray());
        }

        public UInt160 ToScriptHash()
        {
            return new UInt160(Value.ToByteArray().AsSpan()[1..^4]);
        }

        public JObject ToJson()
        {
            var json = new JObject();
            json["value"] = ToAddress();
            return json;
        }
    }
}
