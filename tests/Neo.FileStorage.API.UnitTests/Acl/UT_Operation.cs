// Copyright (C) 2015-2025 The Neo Project.
//
// UT_Operation.cs file belongs to the neo project and is free
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
    public class UT_Operation
    {
        [TestMethod]
        public void TestString()
        {
            Operation op = Operation.Unspecified;
            Assert.AreEqual("Unspecified", op.ToString());
        }
    }
}
