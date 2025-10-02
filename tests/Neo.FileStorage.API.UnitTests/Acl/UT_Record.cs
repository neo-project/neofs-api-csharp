// Copyright (C) 2015-2025 The Neo Project.
//
// UT_Record.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Acl;

namespace Neo.FileStorage.API.UnitTests.Acl
{
    [TestClass]
    public class UT_Record
    {
        [TestMethod]
        public void TestDefault()
        {
            EACLRecord r = new();
            Assert.AreEqual(Action.Unspecified, r.Action);
            Assert.AreEqual(Operation.Unspecified, r.Operation);
            Assert.AreEqual(0, r.Targets.Count);
            Assert.AreEqual(0, r.Filters.Count);
        }
    }
}
