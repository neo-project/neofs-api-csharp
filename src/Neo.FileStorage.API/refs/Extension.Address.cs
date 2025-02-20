// Copyright (C) 2015-2025 The Neo Project.
//
// Extension.Address.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using System;

namespace Neo.FileStorage.API.Refs
{
    public partial class Address
    {
        public Address(ContainerID cid, ObjectID oid)
        {
            ContainerId = cid;
            ObjectId = oid;
        }

        public string String()
        {
            return ContainerId.String() + "/" + ObjectId.String();
        }

        public static Address FromString(string address)
        {
            var parts = address.Split('/');
            if (parts.Length != 2) throw new FormatException("invalid object address string");
            var cid = ContainerID.FromString(parts[0]);
            var oid = ObjectID.FromString(parts[1]);
            return new Address
            {
                ContainerId = cid,
                ObjectId = oid,
            };
        }
    }
}
