using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Netmap;

namespace Neo.FileStorage.API.UnitTests.TestNetmap
{
    [TestClass]
    public class UT_Helper
    {
        [TestMethod]
        public void TestFlatten()
        {
            List<List<Node>> list = new();
            Assert.AreEqual(0, list.Flatten().Count);
        }
    }
}
