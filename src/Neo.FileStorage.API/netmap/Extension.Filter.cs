using System.Linq;
using Neo.IO.Json;

namespace Neo.FileStorage.API.Netmap
{
    public partial class Filter
    {
        public Filter(string name, string key, string value, Operation op, params Filter[] sub_filters)
        {
            name_ = name;
            key_ = key;
            value_ = value;
            op_ = op;
            if (sub_filters != null) filters_.AddRange(sub_filters);
        }

        public JObject ToJson()
        {
            var json = new JObject();
            json["name"] = Name;
            json["key"] = Key;
            json["op"] = Op.ToString();
            json["value"] = Value;
            json["filters"] = new JArray(Filters.Select(p => p.ToJson()));
            return json;
        }
    }
}
