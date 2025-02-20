// Copyright (C) 2015-2025 The Neo Project.
//
// UT_Refs.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Refs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Neo.FileStorage.API.UnitTests.TestRefs
{
    [TestClass]
    public class UT_Refs
    {
        [TestMethod]
        public void TestObjectID()
        {
            var id = ObjectID.FromValue("a9aa4468861473c86e3d2db9d426e37e5858e015a678f7e6a94a12da3569c8b0".HexToBytes());
            Console.WriteLine(id);
            id = new()
            {
                Value = ByteString.CopyFrom(Convert.FromBase64String("9jpT9lKxXQi0IvCF//QK6V/V5/sbjdnr+lz7ROxlzoA="))
            };
            Console.WriteLine(id.String());
        }

        [TestMethod]
        public void TestVersion()
        {
            var version = new Refs.Version
            {
                Major = 1,
                Minor = 1,
            };
            Console.WriteLine(version.ToByteArray().ToHexString());
            Console.WriteLine(version.ToString());
        }

        [TestMethod]
        public void TestOwnerID()
        {
            var owner = new OwnerID
            {
                Value = ByteString.CopyFrom("351f694a2a49229f8e41d24542a0e6a7329b7ed065a113d002".HexToBytes()),
            };
            Console.WriteLine(owner.ToByteArray().ToHexString());
            var owner1 = new OwnerID
            {
                Value = ByteString.CopyFrom("351f694a2a49229f8e41d24542a0e6a7329b7ed065a113d002".HexToBytes()),
            };
            Assert.IsTrue(owner.Value == owner1.Value);
            Assert.IsTrue(owner.Equals(owner1));
        }

        [TestMethod]
        public void TestOwnerIDToScriptHash()
        {
            var key = "L4kWTNckyaWn2QdUrACCJR1qJNgFFGhTCy63ERk7ZK3NvBoXap6t".LoadWif();
            var scriptHash = key.PublicKey().GetScriptHash();
            var owner = OwnerID.FromScriptHash(scriptHash);
            Assert.AreEqual(scriptHash.ToHexString(), owner.ToScriptHash().ToHexString());
        }

        [TestMethod]
        public void TestContainerID()
        {
            var cid = ContainerID.FromString("4pQhKT9XN9Fj1WFoEPAWXenWopjAQLWLtsQNn5q2z56U");
            Console.WriteLine(cid.Value.ToBase64());
        }

        [TestMethod]
        public void TestOwnerIDFromKey()
        {
            string pk = "A/9ltq55E0pNzp0NOdOFHpurTul6v4boHhxbvFDNKCau";
            var address = Convert.FromBase64String(pk).PublicKeyToAddress();
            Console.WriteLine(address);
        }

        [TestMethod]
        public void TestObjectIDDistinct()
        {
            var id1 = ObjectID.FromString("6hc8bGUWr22VKWsnzcEqzw6c5qhDh2cdbziASkrme7tu");
            var id2 = ObjectID.FromString("6hc8bGUWr22VKWsnzcEqzw6c5qhDh2cdbziASkrme7tu");
            var ids = new List<ObjectID>() { id1, id2 };
            Assert.AreEqual(1, ids.Distinct().Count());
        }
    }
}
