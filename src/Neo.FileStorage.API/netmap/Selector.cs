using Neo.IO.Json;

namespace Neo.FileStorage.API.Netmap
{
    public partial class Selector
    {
        public Selector(string name, string attr, Clause clause, uint count, string filter)
        {
            name_ = name;
            attribute_ = attr;
            clause_ = clause;
            count_ = count;
            filter_ = filter;
        }

        public JObject ToJson()
        {
            var json = new JObject();
            json["name"] = Name;
            json["count"] = Count;
            json["clause"] = Clause.ToString();
            json["attribute"] = Attribute;
            json["filter"] = Filter;
            return json;
        }
    }
}
