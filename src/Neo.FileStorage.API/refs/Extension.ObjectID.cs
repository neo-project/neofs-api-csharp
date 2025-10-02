// Copyright (C) 2015-2025 The Neo Project.
//
// Extension.ObjectID.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Google.Protobuf;
using Neo.FileStorage.API.Cryptography;
using static Neo.FileStorage.API.Cryptography.Helper;

namespace Neo.FileStorage.API.Refs
{
    public partial class ObjectID
    {
        public const int ValueSize = Sha256HashLength;

        public static ObjectID FromValue(byte[] hash)
        {
            if (hash.Length != Sha256HashLength) throw new System.FormatException("ObjectID must be a hash256");
            return new ObjectID
            {
                Value = ByteString.CopyFrom(hash)
            };
        }

        public static ObjectID FromString(string id)
        {
            return FromValue(Base58.Decode(id));
        }

        public string String()
        {
            return Base58.Encode(Value.ToByteArray());
        }
    }
}
