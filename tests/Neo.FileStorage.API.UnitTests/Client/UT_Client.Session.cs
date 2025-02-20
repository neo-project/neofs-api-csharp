// Copyright (C) 2015-2025 The Neo Project.
//
// UT_Client.Session.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Cryptography;
using System;
using System.Threading;

namespace Neo.FileStorage.API.UnitTests.FSClient
{
    public partial class UT_Client
    {
        [TestMethod]
        public void TestSessionCreate()
        {
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromSeconds(10));
            var token = client.CreateSession(ulong.MaxValue, context: source.Token).Result;
            Console.WriteLine(token.ToString());
            Assert.AreEqual(key.OwnerID(), token.Body.OwnerId);
            Console.WriteLine($"id={token.Body.Id.ToUUID()}, key={token.Body.SessionKey.ToByteArray().ToHexString()}");
        }
    }
}
