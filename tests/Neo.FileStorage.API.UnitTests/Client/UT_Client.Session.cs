using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Refs;

namespace Neo.FileStorage.API.UnitTests.FSClient
{
    public partial class UT_Client
    {
        [TestMethod]
        public void TestSessionCreate()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            using var client = new Client.Client(key, host);
            var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            var token = client.CreateSession(ulong.MaxValue, context: source.Token).Result;
            Assert.AreEqual(OwnerID.FromScriptHash(key.PublicKey().PublicKeyToScriptHash()), token.Body.OwnerId);
            Console.WriteLine($"id={token.Body.Id.ToUUID()}, key={token.Body.SessionKey.ToByteArray().ToHexString()}");
        }
    }
}
