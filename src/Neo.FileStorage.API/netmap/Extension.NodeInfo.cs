
using System.Linq;
using Neo.IO.Json;

namespace Neo.FileStorage.API.Netmap
{
    public sealed partial class NodeInfo
    {
        public JObject ToJson()
        {
            JObject json = new();
            json["publickey"] = PublicKey.ToByteArray().ToHexString();
            json["address"] = new JArray(Addresses.Select(p => (JObject)p));
            json["attributes"] = Attributes.Select(a =>
            {
                JObject j = new();
                j["key"] = a.Key;
                j["value"] = a.Value;
                return j;
            }).ToArray();
            json["state"] = State.ToString();
            return json;
        }
    }
}
