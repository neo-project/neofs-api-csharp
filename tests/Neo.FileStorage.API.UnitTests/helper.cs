// Copyright (C) 2015-2025 The Neo Project.
//
// helper.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Neo.FileStorage.API.UnitTests
{
    internal static class Helper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] Concat(params byte[][] buffers)
        {
            int length = 0;
            for (int i = 0; i < buffers.Length; i++)
                length += buffers[i].Length;
            byte[] dst = new byte[length];
            int p = 0;
            foreach (byte[] src in buffers)
            {
                Buffer.BlockCopy(src, 0, dst, p, src.Length);
                p += src.Length;
            }
            return dst;
        }

        public static string ToHexString(this byte[] value)
        {
            StringBuilder sb = new();
            foreach (byte b in value)
                sb.AppendFormat("{0:x2}", b);
            return sb.ToString();
        }

        public static byte[] HexToBytes(this string value)
        {
            if (value == null || value.Length == 0)
                return Array.Empty<byte>();
            if (value.Length % 2 == 1)
                throw new FormatException();
            byte[] result = new byte[value.Length / 2];
            for (int i = 0; i < result.Length; i++)
                result[i] = byte.Parse(value.Substring(i * 2, 2), NumberStyles.AllowHexSpecifier);
            return result;
        }
    }
}
