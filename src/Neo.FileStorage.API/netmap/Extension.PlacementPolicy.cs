// Copyright (C) 2015-2025 The Neo Project.
//
// Extension.PlacementPolicy.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

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
