// Copyright (C) 2015-2025 The Neo Project.
//
// ReverseMinNorm.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

namespace Neo.FileStorage.API.Netmap.Normalize
{
    public class ReverseMinNorm : INormalizer
    {
        private readonly double min;

        public ReverseMinNorm(double min)
        {
            this.min = min;
        }

        public double Normalize(double w)
        {
            if (w == 0) return 0;
            return min / w;
        }
    }
}
