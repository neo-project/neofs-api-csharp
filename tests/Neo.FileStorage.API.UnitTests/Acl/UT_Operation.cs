using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Acl;
using static Neo.FileStorage.API.Acl.EACLRecord.Types;

namespace Neo.FileStorage.API.UnitTests.Acl
{
    [TestClass]
    public class UT_Operation
    {
        [TestMethod]
        public void TestString()
        {
            Operation op = Operation.Unspecified;
            Assert.AreEqual("Unspecified", op.ToString());
        }
    }
}
