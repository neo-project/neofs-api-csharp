using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Container;
using System;
using System.Threading;

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
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            try
            {
                var obj = client.GetObject(Address, false, new Client.CallOptions { Ttl = 1 }, source.Token).Result;
            }
            catch (Exception e)
            {
                if (e is AggregateException ae)
                {
                    foreach (var ie in ae.InnerExceptions)
                    {
                        if (ie is RpcException re)
                        {
                            Console.WriteLine(re);
                            Assert.AreEqual(StatusCode.Internal, re.StatusCode);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void TestInvalidRequest()
        {
            var channel = GrpcChannel.ForAddress(host, new() { Credentials = SslCredentials.Insecure });
            var containerClient = new ContainerService.ContainerServiceClient(channel);
            var container = containerClient.Get(new GetRequest());
        }
    }
}
