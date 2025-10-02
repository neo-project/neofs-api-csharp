// Copyright (C) 2015-2025 The Neo Project.
//
// MaxNorm.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

namespace Neo.FileStorage.API.Netmap.Normalize
{
    public class MaxNorm : INormalizer
    {
        private readonly double max;

        public MaxNorm(double max)
        {
            this.max = max;
        }

        public double Normalize(double w)
        {
            if (max == 0) return 0;
            return w / max;
        }
    }
}
