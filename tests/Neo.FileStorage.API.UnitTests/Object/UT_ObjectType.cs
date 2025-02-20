// Copyright (C) 2015-2025 The Neo Project.
//
// UT_ObjectType.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Object;
using Neo.FileStorage.API.Refs;
using System;

namespace Neo.FileStorage.API.UnitTests.TestObject
{
    [TestClass]
    public class UT_ObjectType
    {
        [TestMethod]
        public void TestParse()
        {
            Assert.AreEqual("REGULAR", ObjectType.Regular.String());
            Assert.AreEqual("TOMBSTONE", ObjectType.Tombstone.String());
            Assert.AreEqual("STORAGE_GROUP", ObjectType.StorageGroup.String());
            Assert.ThrowsException<InvalidOperationException>(() => ((ObjectType)3).String());
            Assert.AreEqual(ObjectType.Regular, "REGULAR".ToObjectType());
            Assert.AreEqual(ObjectType.Tombstone, "TOMBSTONE".ToObjectType());
            Assert.AreEqual(ObjectType.StorageGroup, "STORAGE_GROUP".ToObjectType());
            Assert.ThrowsException<InvalidOperationException>(() => "Regular".ToObjectType());
        }

        [TestMethod]
        public void TestJson()
        {
            Header header = new()
            {
                ContainerId = ContainerID.FromValue((new byte[] { 1, 2, 3 }).Sha256())
            };
            Console.WriteLine(header.ToString());
        }
    }
}
