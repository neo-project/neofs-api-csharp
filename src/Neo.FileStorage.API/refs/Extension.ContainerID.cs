using Google.Protobuf;
using Neo.Cryptography;
using Neo.IO.Json;
using static Neo.FileStorage.API.Cryptography.Helper;

namespace Neo.FileStorage.API.Refs
{
    public partial class ContainerID
    {
        public const int ValueSize = Sha256HashLength;

        public static ContainerID FromValue(byte[] hash)
        {
            if (hash.Length != Sha256HashLength) throw new System.FormatException("ContainerID must be a hash256");
            return new ContainerID
            {
                Value = ByteString.CopyFrom(hash)
            };
        }

        public static ContainerID FromString(string id)
        {
            return FromValue(Base58.Decode(id));
        }

        public string String()
        {
            return Base58.Encode(Value.ToByteArray());
        }

        public JObject ToJson()
        {
            var json = new JObject();
            json["value"] = Value.ToBase64();
            return json;
        }
    }
}
