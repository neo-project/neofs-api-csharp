
using Neo.IO.Json;

namespace Neo.FileStorage.API.Refs
{
    public sealed partial class Signature
    {
        public JObject ToJson()
        {
            var json = new JObject();
            json["key"] = Key.ToBase64();
            json["sign"] = Sign.ToBase64();
            return json;
        }
    }
}
