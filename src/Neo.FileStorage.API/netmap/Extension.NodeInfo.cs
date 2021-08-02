
using System.Linq;
using Google.Protobuf;
using Neo.IO.Json;
using static Neo.FileStorage.API.Netmap.NodeInfo.Types;

namespace Neo.FileStorage.API.Netmap
{
    public sealed partial class NodeInfo
    {
        public static NodeInfo FromJson(JObject json)
        {
            NodeInfo ni = new();
            ni.PublicKey = ByteString.FromBase64(json["publicKey"].AsString());
            if (json["addresses"] is not null)
                ni.Addresses.AddRange(((JArray)json["addresses"]).Select(p => p.AsString()));
            if (json["attributes"] is not null)
                ni.Attributes.AddRange(((JArray)json["attributes"]).Select(p => Attribute.FromJson(p)));
            ni.State = System.Enum.Parse<State>(json["state"].AsString());
            return ni;
        }

        public JObject ToJson()
        {
            JObject json = new();
            json["publicKey"] = PublicKey.ToBase64();
            json["addresses"] = new JArray(Addresses.Select(p => (JObject)p));
            json["attributes"] = Attributes.Select(a => a.ToJson()).ToArray();
            json["state"] = State.ToString();
            return json;
        }
    }
}
