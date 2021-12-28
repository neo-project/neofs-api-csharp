using Google.Protobuf;
using Neo.FileStorage.API.Refs;
using Neo.FileStorage.API.Cryptography;
using Neo.IO.Json;
using System;
using System.Linq;

namespace Neo.FileStorage.API.Container
{
    public partial class Container
    {
        public const string AttributeName = "Name";
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
                private const string SysAttributePrefix = "__NEOFS__";
                public const string SysAttributeName = SysAttributePrefix + "NAME";
                public const string SysAttributeZone = SysAttributePrefix + "ZONE";
                public const string SysAttributeZoneDefault = "container";

                public JObject ToJson()
                {
                    var json = new JObject();
                    json["key"] = Key;
                    json["value"] = Value;
                    return json;
                }
            }
        }

        public string NativeName
        {
            get
            {
                foreach (var attr in Attributes)
                    if (attr.Key == Types.Attribute.SysAttributeName)
                        return attr.Value;
                return "";
            }

            set
            {
                foreach (var attr in Attributes)
                    if (attr.Key == Types.Attribute.SysAttributeName)
                    {
                        attr.Value = value;
                        return;
                    }
                Attributes.Add(new Types.Attribute
                {
                    Key = Types.Attribute.SysAttributeName,
                    Value = value,
                });
            }
        }

        public string NativeZone
        {
            get
            {
                foreach (var attr in Attributes)
                    if (attr.Key == Types.Attribute.SysAttributeZone)
                        return attr.Value;
                return "";
            }

            set
            {
                foreach (var attr in Attributes)
                    if (attr.Key == Types.Attribute.SysAttributeZone)
                    {
                        attr.Value = value;
                        return;
                    }
                Attributes.Add(new Types.Attribute
                {
                    Key = Types.Attribute.SysAttributeZone,
                    Value = value,
                });
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
