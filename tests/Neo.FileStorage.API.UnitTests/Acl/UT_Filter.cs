using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Acl;
using static Neo.FileStorage.API.Acl.EACLRecord.Types;

namespace Neo.FileStorage.API.UnitTests.Acl
{
    [TestClass]
    public class UT_Filter
    {
        [TestMethod]
        public void TestDefault()
        {
            Filter f = new();
            Assert.AreEqual(MatchType.Unspecified, f.MatchType);
            Assert.AreEqual(HeaderType.HeaderUnspecified, f.HeaderType);
            Assert.AreEqual("", f.Key);
            Assert.AreEqual("", f.Value);
        }
    }
}
