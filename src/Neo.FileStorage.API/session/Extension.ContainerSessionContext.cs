using Neo.IO.Json;

namespace Neo.FileStorage.API.Session
{
    public sealed partial class ContainerSessionContext
    {
        public JObject ToJson()
        {
            var json = new JObject();
            json["verb"] = Verb.ToString();
            json["wildcard"] = Wildcard.ToString();
            json["containerID"] = ContainerId.ToJson();
            return json;
        }
    }
}
