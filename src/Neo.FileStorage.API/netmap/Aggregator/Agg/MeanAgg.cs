// Copyright (C) 2015-2025 The Neo Project.
//
// MeanAgg.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

namespace Neo.FileStorage.API.Netmap.Aggregator
{
    public class MeanAgg : IAggregator
    {
        private double mean;
        private int count;

        public void Add(double n)
        {
            var c = count + 1;
            mean = mean * ((double)count / (double)c) + n / c;
            count++;
        }

        public double Compute()
        {
            return mean;
        }

        public void Clear()
        {
            mean = 0;
            count = 0;
        }
    }
}
