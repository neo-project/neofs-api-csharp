using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Neo.FileStorage.API.UnitTests.FSClient
{
    public partial class UT_Client
    {
        [TestMethod]
        public void TestLocalNodeInfo()
        {
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var ni = client.LocalNodeInfo(context: source.Token).Result;
            Console.WriteLine(ni.ToString());
        }

        [TestMethod]
        public void TestEpoch()
        {
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var epoch = client.Epoch(context: source.Token).Result;
            Console.WriteLine(epoch);
            Assert.IsTrue(0 < epoch);
        }

        [TestMethod]
        public void TestVersion()
        {
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var epoch = client.Version(context: source.Token).Result;
            Console.WriteLine(epoch);
        }

        [TestMethod]
        public void TestNetworkInfo()
        {
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var ni = client.NetworkInfo(context: source.Token).Result;
            Console.WriteLine(ni.ToString());
        }
    }
}
