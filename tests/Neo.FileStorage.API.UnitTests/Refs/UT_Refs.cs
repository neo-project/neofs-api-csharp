using System;
using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Refs;

namespace Neo.FileStorage.API.UnitTests.TestRefs
{
    [TestClass]
    public class UT_Refs
    {
        [TestMethod]
        public void TestObjectID()
        {
            var id = ObjectID.FromSha256Bytes("a9aa4468861473c86e3d2db9d426e37e5858e015a678f7e6a94a12da3569c8b0".HexToBytes());
            Console.WriteLine(id);
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
        public void TestContainerID()
        {
            var cid = ContainerID.FromBase58String("5Cyxb3wrHDw5pqY63hb5otCSsJ24ZfYmsA8NAjtho2gr");
            var oid = ObjectID.FromBase58String("5Cyxb3wrHDw5pqY63hb5otCSsJ24ZfYmsA8NAjtho2gr");
        }
    }
}