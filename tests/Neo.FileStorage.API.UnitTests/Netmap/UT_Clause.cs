using System;
using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            c = Enum.Parse<Netmap.Clause>("same", true);
            Assert.AreEqual(Netmap.Clause.Same, c);
        }
    }
}
