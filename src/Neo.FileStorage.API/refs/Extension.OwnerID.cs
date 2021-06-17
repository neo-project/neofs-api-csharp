using System;
using Google.Protobuf;
using Neo.Cryptography;
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

        public static OwnerID FromBase58String(string id)
        {
            return FromByteArray(Base58.Decode(id));
        }

        public string ToBase58String()
        {
            return Base58.Encode(Value.ToByteArray());
        }

        public JObject ToJson()
        {
            var json = new JObject();
            json["value"] = ToBase58String();
            return json;
        }
    }
}
