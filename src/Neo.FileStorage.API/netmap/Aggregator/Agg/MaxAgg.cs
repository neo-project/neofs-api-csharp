// Copyright (C) 2015-2025 The Neo Project.
//
// MaxAgg.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

namespace Neo.FileStorage.API.Netmap.Aggregator
{
    public class MaxAgg : IAggregator
    {
        private double max = 0;

        public void Add(double n)
        {
            if (max < n)
                max = n;
        }

        public double Compute()
        {
            return max;
        }

        public void Clear()
        {
            max = 0;
        }
    }
}
