using System;
using System.Threading;
using Grpc.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Refs;

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
                Console.WriteLine(e);
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

        [TestMethod]
        public void TestObjectGetException()
        {
            ObjectID coid = ObjectID.FromString("8Bhi84qyNBHkCPdfRAuEkU9bmbZTZsQJbyJnpJQiPY45");
            var address = new Address(cid, coid);
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            try
            {
                var obj = client.GetObject(address, false, new Client.CallOptions { Ttl = 2 }, source.Token).Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (e is AggregateException ae)
                {
                    foreach (var ie in ae.InnerExceptions)
                    {
                        if (ie is RpcException re)
                        {
                            Assert.AreEqual(StatusCode.Unknown, re.StatusCode);
                            Assert.AreEqual("object already removed", re.Status.Detail);
                        }
                    }
                }
            }
        }
    }
}
