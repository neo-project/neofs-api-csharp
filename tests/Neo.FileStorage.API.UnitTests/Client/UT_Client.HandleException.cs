using System;
using System.Threading;
using Grpc.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Neo.FileStorage.API.UnitTests.FSClient
{
    public partial class UT_Client
    {
        [TestMethod]
        public void TestBalanceException()
        {
            using var client = new Client.Client(key, host);
            using CancellationTokenSource source = new();
            source.Cancel();
            try
            {
                var balance = client.GetBalance(context: source.Token).Result;
                Assert.AreEqual(0, balance.Value);
            }
            catch (Exception e)
            {
                if (e is AggregateException ae)
                {
                    foreach (var ie in ae.InnerExceptions)
                    {
                        if (ie is RpcException re)
                        {
                            Assert.AreEqual(StatusCode.Cancelled, re.StatusCode);
                        }
                    }
                }
            }
        }
    }
}
