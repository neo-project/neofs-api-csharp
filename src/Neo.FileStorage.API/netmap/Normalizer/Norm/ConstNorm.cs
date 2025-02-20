// Copyright (C) 2015-2025 The Neo Project.
//
// ConstNorm.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

namespace Neo.FileStorage.API.Netmap.Normalize
{
    public class ConstNorm : INormalizer
    {
        private readonly double value;

        public ConstNorm(double value)
        {
            this.value = value;
        }

        public double Normalize(double w)
        {
            return value;
        }
    }
}
