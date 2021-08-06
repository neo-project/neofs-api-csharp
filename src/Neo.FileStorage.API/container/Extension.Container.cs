using Google.Protobuf;
using Neo.FileStorage.API.Acl;
using Neo.FileStorage.API.Refs;
using Neo.FileStorage.API.Cryptography;
using Neo.IO.Json;
using System;
using System.Linq;

namespace Neo.FileStorage.API.Container
{
    public partial class Container
    {
        // AttributeName is an attribute key that is commonly used to denote
        // human-friendly name.
        public const string AttributeName = "Name";

        // AttributeTimestamp is an attribute key that is commonly used to denote
        // user-defined local time of container creation in Unix Timestamp format.
        public const string AttributeTimestamp = "Timestamp";

        private ContainerID _id;
        public ContainerID CalCulateAndGetId
        {
            get
            {
                if (_id is null)
                    _id = new ContainerID
                    {
                        Value = this.Sha256()
                    };
                return _id;
            }
        }

        public Guid NonceUUID
        {
            get
            {
                return new Guid(nonce_.ToByteArray());
            }
            set
            {
                nonce_ = ByteString.CopyFrom(value.ToByteArray());
            }
        }

        public static partial class Types
        {
            public sealed partial class Attribute
            {
                // SysAttributePrefix is a prefix of key to system attribute.
                public const string SysAttributePrefix = "__NEOFS__";

                // SysAttributeSubnet is a string ID of container's storage subnet.
                public const string SysAttributeSubnet = SysAttributePrefix + "SUBNET";

                public JObject ToJson()
                {
                    var json = new JObject();
                    json["key"] = Key;
                    json["value"] = Value;
                    return json;
                }
            }
        }

        public JObject ToJson()
        {
            var json = new JObject();
            json["version"] = Version?.ToJson();
            json["ownerID"] = OwnerId?.ToJson();
            json["nonce"] = Nonce.ToBase64();
            json["basicACL"] = BasicAcl;
            json["attributes"] = new JArray(Attributes.Select(p => p.ToJson()));
            json["placementPolicy"] = PlacementPolicy?.ToJson();
            return json;
        }
    }
}