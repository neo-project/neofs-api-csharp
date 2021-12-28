using Neo.IO.Json;
using System.Linq;

namespace Neo.FileStorage.API.Netmap
{
    public sealed partial class NodeInfo
    {
        public static partial class Types
        {
            public sealed partial class Attribute
            {
                private const string SysAttributePrefix = "__NEOFS__";
                public const string SysAttributeSubnet = SysAttributePrefix + "SUBNET_";

                public const string AttrSubnetValExit = "FALSE";
                public const string AttrSubnetValEntry = "TRUE";

                public static Attribute FromJson(JObject json)
                {
                    Attribute attribute = new();
                    attribute.Key = json["key"].AsString();
                    attribute.Value = json["value"].AsString();
                    if (json["parents"] is not null)
                        attribute.Parents.AddRange(((JArray)json["parents"]).Select(p => p.AsString()));
                    return attribute;
                }

                public JObject ToJson()
                {
                    JObject json = new();
                    json["key"] = Key;
                    json["value"] = Value;
                    return json;
                }
            }
        }
    }
}
