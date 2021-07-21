using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Cryptography;

namespace Neo.FileStorage.API.UnitTests.FSClient
{
    public partial class UT_Client
    {
        [TestMethod]
        public void TestBalance()
        {
            var host = "http://localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            using var client = new Client.Client(key, host);
            var balance = client.GetBalance().Result;
            Assert.AreEqual(0, balance.Value);
        }
    }
}
