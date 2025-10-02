// Copyright (C) 2015-2025 The Neo Project.
//
// UT_Client.Accounting.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Refs;

namespace Neo.FileStorage.API.UnitTests.FSClient
{
    public partial class UT_Client
    {
        [TestMethod]
        public void TestBalance()
        {
            using var client = new Client.Client(key, host);
            var balance = client.GetBalance().Result;
            Assert.AreEqual(12u, balance.Precision);
            Assert.AreEqual(0, balance.Value);
        }

        [TestMethod]
        public void TestBalanceWithOwner()
        {
            var address = "NiXweMv91Vz512bQw7jFNHAGBg8upVS8Qo";
            using var client = new Client.Client(key, host);
            var balance = client.GetBalance(OwnerID.FromAddress(address)).Result;
            Assert.AreEqual(0, balance.Value);
        }
    }
}
