// Copyright (C) 2015-2025 The Neo Project.
//
// MeanSumAgg.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

namespace Neo.FileStorage.API.Netmap.Aggregator
{
    public class MeanSumAgg : IAggregator
    {
        private double sum;
        private int count;

        public void Add(double n)
        {
            sum += n;
            count++;
        }

        public double Compute()
        {
            if (count == 0) return 0;
            return sum / count;
        }

        public void Clear()
        {
            sum = 0;
            count = 0;
        }
    }
}
