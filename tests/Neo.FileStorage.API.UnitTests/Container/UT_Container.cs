using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Container;
using Neo.FileStorage.API.Netmap;
using Neo.FileStorage.API.Refs;
using System;

namespace Neo.FileStorage.API.UnitTests.TestContainer
{
    [TestClass]
    public class UT_Container
    {
        [TestMethod]
        public void TestContainerSerialize()
        {
            var key = "Kwk6k2eC3L3QuPvD8aiaNyoSXgQ2YL1bwS5CP1oKoA9waeAze97s".LoadWif();
            var container = new Container.Container
            {
                Version = new Refs.Version
                {
                    Major = 1,
                    Minor = 2,
                },
                OwnerId = key.ToOwnerID(),
                Nonce = ByteString.CopyFrom("1234".HexToBytes()),
                BasicAcl = 0u,
                PlacementPolicy = new PlacementPolicy(1, null, null, null),
            };
            Assert.AreEqual("0a0408011002121b0a19351f694a2a49229f8e41d24542a0e6a7329b7ed065a113d0021a02123432021001", (container.ToByteArray().ToHexString()));
        }
    }
}
