// Copyright (C) 2015-2025 The Neo Project.
//
// Extension.PeerID.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Google.Protobuf;
using Neo.FileStorage.API.Cryptography;
using System;

namespace Neo.FileStorage.API.Reputation
{
    public sealed partial class PeerID
    {
        public const int PeerIDLength = 33;

        public PeerID(byte[] bytes)
        {
            if (bytes is null || bytes.Length != PeerIDLength)
                throw new ArgumentException("invalid PeerID bytes", nameof(bytes));
            PublicKey = ByteString.CopyFrom(bytes);
        }

        public static PeerID Parse(string idString)
        {
            return new PeerID(Base58.Decode(idString));
        }

        public string String()
        {
            return Base58.Encode(PublicKey.ToByteArray());
        }

        public static implicit operator PeerID(byte[] value)
        {
            return new PeerID(value);
        }

        public static implicit operator byte[](PeerID pid)
        {
            return pid.PublicKey.ToByteArray();
        }
    }
}
