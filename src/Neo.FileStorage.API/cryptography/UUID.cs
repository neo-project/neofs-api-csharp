// Copyright (C) 2015-2025 The Neo Project.
//
// UUID.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Google.Protobuf;
using System;

namespace Neo.FileStorage.API.Cryptography
{
    public static class UUIDExtension
    {

        public static Guid ToUUID(this ByteString id)
        {
            return Guid.Parse(id.ToByteArray().ToHexString());
        }

        public static ByteString ToByteString(this Guid id)
        {
            return ByteString.CopyFrom(id.Bytes());
        }

        public static byte[] Bytes(this Guid id)
        {
            return id.ToString().Replace("-", String.Empty).HexToBytes();
        }
    }
}
