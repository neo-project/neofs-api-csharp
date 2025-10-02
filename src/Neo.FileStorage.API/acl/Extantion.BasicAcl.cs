// Copyright (C) 2015-2025 The Neo Project.
//
// Extantion.BasicAcl.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using System.Collections.Generic;

namespace Neo.FileStorage.API.Acl
{
    public static class BasicAcl
    {
        /// <summary>
        /// PublicBasicRule is a basic ACL value for public-read-write container.
        /// </summary>
        public const uint PublicBasicRule = 0x1FBFBFFF;

        /// <summary>
        /// PrivateBasicRule is a basic ACL value for private container.
        /// </summary>
        public const uint PrivateBasicRule = 0x1C8C8CCC;

        /// <summary>
        /// ReadOnlyBasicRule is a basic ACL value for public-read container.
        /// </summary>
        public const uint ReadOnlyBasicRule = 0x1FBF8CFF;

        /// <summary>
        /// PublicAppendRule is a basic ACL value for public-append container.
        /// </summary>
        public const uint PublicAppendRule = 0x1FBF9FFF;

        private const int ReservedBitNumber = 2;
        private const int StickyBitPos = ReservedBitNumber;
        private const int FinalBitPos = StickyBitPos + 1;
        private const int OpOffset = FinalBitPos + 1;
        private const int BitsPerOp = 4;
        private const int OpNumber = 7;
        private const int LeftACLBitPos = OpOffset + BitsPerOp * OpNumber - 1;

        private const byte BitUser = 0;
        private const byte BitSystem = 1;
        private const byte BitOthers = 2;
        private const byte BitBearer = 3;

        private static readonly Dictionary<Operation, byte> Order = new()
        {
            { Operation.Getrangehash, 0 },
            { Operation.Getrange, 1 },
            { Operation.Search, 2 },
            { Operation.Delete, 3 },
            { Operation.Put, 4 },
            { Operation.Head, 5 },
            { Operation.Get, 6 },
        };

        private static bool IsLeftBitSet(this uint value, byte n)
        {
            var bitMask = (uint)(1 << (LeftACLBitPos - n));
            return bitMask != 0 && (value & bitMask) == bitMask;
        }

        private static void SetLeftBit(this ref uint value, byte n)
        {
            value |= (uint)(1 << (LeftACLBitPos - n));
        }

        private static void ResetLeftBit(this ref uint value, byte n)
        {
            value &= ~(uint)(1 << (LeftACLBitPos - n));
        }

        public static bool Final(this uint value)
        {
            return IsLeftBitSet(value, FinalBitPos);
        }

        public static void SetFinal(this ref uint value)
        {
            value.SetLeftBit(FinalBitPos);
        }

        public static void ResetFinal(this ref uint value)
        {
            value.ResetLeftBit(FinalBitPos);
        }

        public static bool Sticky(this uint value)
        {
            return value.IsLeftBitSet(StickyBitPos);
        }

        public static void SetSticky(this ref uint value)
        {
            value.SetLeftBit(StickyBitPos);
        }

        public static void ResetSticky(this ref uint value)
        {
            value.ResetLeftBit(StickyBitPos);
        }

        public static bool UserAllowed(this uint value, Operation op)
        {
            if (Order.TryGetValue(op, out byte n))
            {
                return value.IsLeftBitSet((byte)(OpOffset + n * BitsPerOp + BitUser));
            }
            return false;
        }

        public static void AllowUser(this ref uint value, Operation op)
        {
            if (Order.TryGetValue(op, out byte n))
            {
                value.SetLeftBit((byte)(OpOffset + n * BitsPerOp + BitUser));
            }
        }

        public static void ForbidUser(this ref uint value, Operation op)
        {
            if (Order.TryGetValue(op, out byte n))
            {
                value.ResetLeftBit((byte)(OpOffset + n * BitsPerOp + BitUser));
            }
        }

        public static bool SystemAllowed(this uint value, Operation op)
        {
            if (op != Operation.Delete && op != Operation.Getrange) return true;
            if (Order.TryGetValue(op, out byte n))
            {
                return value.IsLeftBitSet((byte)(OpOffset + n * BitsPerOp + BitSystem));
            }
            return false;
        }

        public static void AllowSystem(this ref uint value, Operation op)
        {
            if (Order.TryGetValue(op, out byte n))
            {
                value.SetLeftBit((byte)(OpOffset + n * BitsPerOp + BitSystem));
            }
        }

        public static void ForbidSystem(this ref uint value, Operation op)
        {
            if (Order.TryGetValue(op, out byte n))
            {
                value.ResetLeftBit((byte)(OpOffset + n * BitsPerOp + BitSystem));
            }
        }

        public static bool InnerRingAllowed(this uint value, Operation op)
        {
            if (op == Operation.Search || op == Operation.Getrangehash || op == Operation.Head) return true;
            if (Order.TryGetValue(op, out byte n))
            {
                return value.IsLeftBitSet((byte)(OpOffset + n * BitsPerOp + BitSystem));
            }
            return false;
        }

        public static bool OthersAllowed(this uint value, Operation op)
        {
            if (Order.TryGetValue(op, out byte n))
            {
                return value.IsLeftBitSet((byte)(OpOffset + n * BitsPerOp + BitOthers));
            }
            return false;
        }

        public static void AllowOthers(this ref uint value, Operation op)
        {
            if (Order.TryGetValue(op, out byte n))
            {
                value.SetLeftBit((byte)(OpOffset + n * BitsPerOp + BitOthers));
            }
        }

        public static void ForbidOthers(this ref uint value, Operation op)
        {
            if (Order.TryGetValue(op, out byte n))
            {
                value.ResetLeftBit((byte)(OpOffset + n * BitsPerOp + BitOthers));
            }
        }

        public static bool BearsAllowed(this uint value, Operation op)
        {
            if (Order.TryGetValue(op, out byte n))
            {
                return value.IsLeftBitSet((byte)(OpOffset + n * BitsPerOp + BitBearer));
            }
            return false;
        }

        public static void AllowBears(this ref uint value, Operation op)
        {
            if (Order.TryGetValue(op, out byte n))
            {
                value.SetLeftBit((byte)(OpOffset + n * BitsPerOp + BitBearer));
            }
        }

        public static void ForbidBears(this ref uint value, Operation op)
        {
            if (Order.TryGetValue(op, out byte n))
            {
                value.ResetLeftBit((byte)(OpOffset + n * BitsPerOp + BitBearer));
            }
        }
    }
}
