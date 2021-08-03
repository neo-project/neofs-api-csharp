using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Neo.FileStorage.API.UnitTests.FSClient
{
    public partial class UT_Client
    {
        [TestMethod]
        public void TestBalance()
        {
            using var client = new Client.Client(key, host);
            var balance = client.GetBalance().Result;
            Assert.AreEqual(0, balance.Value);
        }
    }
}
