

using System;
using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Object;

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
    }
}
