// Copyright (C) 2015-2025 The Neo Project.
//
// UT_Status.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Status;

namespace Neo.FileStorage.API.UnitTests.Status
{
    [TestClass]
    public class UT_Status
    {
        [TestMethod]
        public void TestGlobalize()
        {
            var code1 = Success.Ok;
            Assert.AreEqual(0u, API.Status.Status.Globalize(code1));
            var code2 = CommonFail.Internal;
            Assert.AreEqual(1024u, API.Status.Status.Globalize(code2));
            var code3 = API.Status.Object.NotFound;
            Assert.AreEqual(2049u, API.Status.Status.Globalize(code3));
        }

        [TestMethod]
        public void TestLocalize()
        {
            Assert.AreEqual(Success.Ok, API.Status.Status.Localize(0));
            Assert.AreEqual(CommonFail.Internal, API.Status.Status.Localize(1024));
        }
    }
}
