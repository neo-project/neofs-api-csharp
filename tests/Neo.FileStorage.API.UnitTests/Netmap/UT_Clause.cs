using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Netmap;
using System;

namespace Neo.FileStorage.API.UnitTests.TestNetmap
{
    [TestClass]
    public class UT_Clause
    {
        [TestMethod]
        public void TestString()
        {
            var c = Neo.FileStorage.API.Netmap.Clause.Same;
            Assert.AreEqual("Same", c.ToString());
        }
    }
}