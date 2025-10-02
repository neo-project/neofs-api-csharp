// Copyright (C) 2015-2025 The Neo Project.
//
// UT_NetmapState.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Netmap;
using System;

namespace Neo.FileStorage.API.UnitTests.TestNetmap
{
    [TestClass]
    public class UT_NetmapState
    {
        [TestMethod]
        public void TestString()
        {
            NodeInfo.Types.State state = NodeInfo.Types.State.Offline;
            Assert.AreEqual("Offline", state.ToString());
            state = Enum.Parse<NodeInfo.Types.State>("Offline");
            Assert.AreEqual(NodeInfo.Types.State.Offline, state);
        }
    }
}
