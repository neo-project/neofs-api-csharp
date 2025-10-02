// Copyright (C) 2015-2025 The Neo Project.
//
// Extension.ObjectType.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using System;
using System.Collections.Generic;

namespace Neo.FileStorage.API.Object
{
    public static class ObjectTypeExtension
    {
        private static readonly Dictionary<ObjectType, string> ObjectTypeNames = new()
        {
            { ObjectType.Regular, "REGULAR" },
            { ObjectType.Tombstone, "TOMBSTONE" },
            { ObjectType.StorageGroup, "STORAGE_GROUP" }
        };
        private static readonly Dictionary<string, ObjectType> ObjectTypeValues = new()
        {
            { "REGULAR", ObjectType.Regular },
            { "TOMBSTONE", ObjectType.Tombstone },
            { "STORAGE_GROUP", ObjectType.StorageGroup }
        };

        public static string String(this ObjectType t)
        {
            if (ObjectTypeNames.TryGetValue(t, out var name))
                return name;
            throw new InvalidOperationException("Invalid object type");
        }

        public static ObjectType ToObjectType(this string t)
        {
            if (ObjectTypeValues.TryGetValue(t, out var type))
                return type;
            throw new InvalidOperationException("Invalid object type string");
        }
    }
}
