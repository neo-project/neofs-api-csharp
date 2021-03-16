using Neo.IO.Json;

namespace Neo.FileStorage.API.Netmap
{
    public partial class Replica
    {
        public Replica(uint c, string s)
        {
            count_ = c;
            selector_ = s;
        }

        public JObject ToJson()
        {
            var json = new JObject();
            json["count"] = Count;
            json["selector"] = Selector;
            return json;
        }
    }
}
