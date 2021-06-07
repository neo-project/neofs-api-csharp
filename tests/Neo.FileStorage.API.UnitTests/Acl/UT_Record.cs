using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Acl;

namespace Neo.FileStorage.API.UnitTests.Acl
{
    [TestClass]
    public class UT_Record
    {
        [TestMethod]
        public void TestDefault()
        {
            EACLRecord r = new();
            Assert.AreEqual(Action.Unspecified, r.Action);
            Assert.AreEqual(Operation.Unspecified, r.Operation);
            Assert.AreEqual(0, r.Targets.Count);
            Assert.AreEqual(0, r.Filters.Count);
        }
    }
}
