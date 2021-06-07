using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Acl;
using static Neo.FileStorage.API.Acl.EACLRecord.Types;

namespace Neo.FileStorage.API.UnitTests.Acl
{
    [TestClass]
    public class UT_Target
    {
        [TestMethod]
        public void TestDefault()
        {
            Target r = new();
            Assert.AreEqual(Role.Unspecified, r.Role);
            Assert.AreEqual(0, r.Keys.Count);
        }
    }
}
