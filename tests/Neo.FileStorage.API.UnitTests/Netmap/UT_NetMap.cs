using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Netmap;
using System;
using System.Collections.Generic;

namespace Neo.FileStorage.API.UnitTests.TestNetmap
{
    [TestClass]
    public class UT_NetMap
    {
        [TestMethod]
        public void TestFlattenNodes()
        {
            var ns1 = new List<Node> { Helper.GenerateTestNode(0, ("Raing", "1")) };
            var ns2 = new List<Node> { Helper.GenerateTestNode(0, ("Raing", "2")) };
            List<List<Node>> list = new();
            list.Add(ns1);
            list.Add(ns2);
            Assert.AreEqual(2, list.Flatten().Count);
        }
    }
}