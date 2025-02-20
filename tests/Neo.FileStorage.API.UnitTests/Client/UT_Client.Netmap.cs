// Copyright (C) 2015-2025 The Neo Project.
//
// UT_Client.Netmap.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

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
