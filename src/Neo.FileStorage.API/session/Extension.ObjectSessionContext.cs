using Neo.IO.Json;

namespace Neo.FileStorage.API.Session
{
    public sealed partial class ObjectSessionContext
    {
        public JObject ToJson()
        {
            var json = new JObject();
            json["verb"] = Verb.ToString();
            json["address"] = Address?.ToJson();
            return json;
        }
    }
}
