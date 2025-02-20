// Copyright (C) 2015-2025 The Neo Project.
//
// UT_Target.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Acl;
using static Neo.FileStorage.API.Acl.EACLRecord.Types;

namespace Neo.FileStorage.API.UnitTests.Acl
{
    [TestClass]
    public class UT_Target
    {
        [TestMethod]
        public void TestDefault()
        {
            Target r = new();
            Assert.AreEqual(Role.Unspecified, r.Role);
            Assert.AreEqual(0, r.Keys.Count);
        }
    }
}
