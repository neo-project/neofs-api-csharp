using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Control;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Refs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Neo.FileStorage.API.UnitTests
{
    [TestClass]
    public class UT_ControlClient
    {
        private readonly ECDsa key = "Kzj1LbTtmfbyJjn4cZhD6U4pdq74iHcmKmGRRBiLQoQzPBRWLEKz".LoadWif();
        private readonly string host = "http://localhost:8080";

        [TestMethod]
        public void TestDropObjects()
        {
            List<string> addresses = new() { "MLFfaXvfRNdJuCBBym4aTWuUMpBVaoEsbupgeZxbLNF/C3dicb6rCdmsZF6remRptWhMgfymdCjF4XAvPFk8XoFA" };
            var client = new ControlClient(key, host);
            client.DropObjects(addresses.Select(p => Address.ParseString(p).ToByteString()).ToArray());
            Console.WriteLine("Success!");
        }

        [TestMethod]
        public void TestHealthCheck()
        {
            var client = new ControlClient(key, host);
            var status = client.HealthCheck();
            Console.WriteLine(status);
            Assert.AreEqual(HealthStatus.Ready, status);
        }

        [TestMethod]
        public void TestNetmapSnapshot()
        {
            var client = new ControlClient(key, host);
            var netmap = client.NetmapSnapshot();
            Console.WriteLine(netmap);
        }

        [TestMethod]
        public void TestSetNetmapStatus()
        {
            var client = new ControlClient(key, host);
            client.SetNetmapStatusRequest(NetmapStatus.Online);
            Console.WriteLine("Success!");
        }
    }
}
