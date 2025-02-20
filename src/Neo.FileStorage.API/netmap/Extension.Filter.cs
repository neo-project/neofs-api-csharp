// Copyright (C) 2015-2025 The Neo Project.
//
// Extension.Filter.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

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
    }
}
