using System;
using System.Security.Cryptography;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Cryptography;

namespace Neo.FileStorage.API.UnitTests.FSClient
{
    [TestClass]
    public class UT_Netmap
    {
        private readonly string host = "http://localhost:8080";
        private readonly ECDsa key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();

        [TestMethod]
        public void TestLocalNodeInfo()
        {
            var client = new Client.Client(key, host);
            var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var ni = client.LocalNodeInfo(context: source.Token).Result;
            Console.WriteLine(ni.ToString());
        }

        [TestMethod]
        public void TestEpoch()
        {
            var client = new Client.Client(key, host);
            var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var epoch = client.Epoch(context: source.Token).Result;
            Console.WriteLine(epoch);
            Assert.Fail();
        }
    }
}
