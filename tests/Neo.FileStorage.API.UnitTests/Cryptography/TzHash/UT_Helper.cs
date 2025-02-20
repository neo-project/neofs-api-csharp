// Copyright (C) 2015-2025 The Neo Project.
//
// UT_Helper.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Neo.FileStorage.API.Cryptography.Tz.Helper;

namespace Neo.FileStorage.API.UnitTests.TestCryptography.Tz
{
    [TestClass]
    public class UT_Helper
    {
        [TestMethod]
        public void TestGetLeadingZeros()
        {
            ulong u1 = 1;
            int i = GetLeadingZeros(u1);
            Assert.AreEqual(63, i);
        }
    }
}
