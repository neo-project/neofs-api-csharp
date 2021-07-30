using System.Linq;
using Neo.IO.Json;

namespace Neo.FileStorage.API.Netmap
{
    public partial class PlacementPolicy
    {
        public PlacementPolicy(uint cbf, Replica[] replicas, Selector[] selectors, Filter[] filters)
        {
            containerBackupFactor_ = cbf;
            if (replicas != null) replicas_.AddRange(replicas);
            if (selectors != null) selectors_.AddRange(selectors);
            if (filters != null) filters_.AddRange(filters);
        }

        public JObject ToJson()
        {
            var json = new JObject();
            json["replicas"] = new JArray(Replicas.Select(p => p.ToJson()));
            json["containerBackupFactor"] = ContainerBackupFactor;
            json["selectors"] = new JArray(Selectors.Select(p => p.ToJson()));
            json["filters"] = new JArray(Filters.Select(p => p.ToJson()));
            return json;
        }
    }
}
