// Copyright (C) 2015-2025 The Neo Project.
//
// Helper.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using System;
using System.Security.Cryptography;

namespace Neo.FileStorage.API.Cryptography.Tz
{
    public static class Helper
    {
        public static ulong NextUlong(this RandomNumberGenerator rng)
        {
            var buff = new byte[8];
            rng.GetBytes(buff);
            return BitConverter.ToUInt64(buff, 0);
        }

        public static int GetLeadingZeros(ulong value)
        {
            int i = 64;
            while (value != 0)
            {
                value >>= 1;
                i--;
            }
            return i;
        }

        public static int GetNonZeroLength(this ulong value)
        {
            return 64 - GetLeadingZeros(value);
        }
    }
}
