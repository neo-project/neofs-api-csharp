using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using Neo.FileStorage.API.Netmap;

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
