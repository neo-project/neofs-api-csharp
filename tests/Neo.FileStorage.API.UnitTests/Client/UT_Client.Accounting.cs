using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Refs;

namespace Neo.FileStorage.API.UnitTests.FSClient
{
    public partial class UT_Client
    {
        [TestMethod]
        public void TestBalance()
        {
            using var client = new Client.Client(key, host);
            var balance = client.GetBalance().Result;
            Assert.AreEqual(12u, balance.Precision);
            Assert.AreEqual(0, balance.Value);
        }

        [TestMethod]
        public void TestBalanceWithOwner()
        {
            var address = "NiXweMv91Vz512bQw7jFNHAGBg8upVS8Qo";
            using var client = new Client.Client(key, host);
            var balance = client.GetBalance(OwnerID.FromAddress(address)).Result;
            Assert.AreEqual(0, balance.Value);
        }
    }
}
