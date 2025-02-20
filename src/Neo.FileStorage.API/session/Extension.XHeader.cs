// Copyright (C) 2015-2025 The Neo Project.
//
// Extension.XHeader.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

namespace Neo.FileStorage.API.Session
{
    public partial class XHeader
    {
        public const string ReservedXHeaderPrefix = "__NEOFS__";
        public const string XHeaderNetmapEpoch = ReservedXHeaderPrefix + "NETMAP_EPOCH";
        public const string XHeaderNetmapLookupDepth = ReservedXHeaderPrefix + "NETMAP_LOOKUP_DEPTH";
    }
}
