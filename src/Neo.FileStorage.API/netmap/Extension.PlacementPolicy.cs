
using System.Collections.Generic;

namespace Neo.FileStorage.API.Netmap
{
    public partial class PlacementPolicy
    {
        public PlacementPolicy(uint cbf, IEnumerable<Replica> replicas, IEnumerable<Selector> selectors, IEnumerable<Filter> filters)
        {
            containerBackupFactor_ = cbf;
            if (replicas != null) replicas_.AddRange(replicas);
            if (selectors != null) selectors_.AddRange(selectors);
            if (filters != null) filters_.AddRange(filters);
        }
    }
}
