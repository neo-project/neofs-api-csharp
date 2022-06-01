using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Netmap;
using Neo.FileStorage.API.Refs;
using Newtonsoft.Json.Linq;

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
