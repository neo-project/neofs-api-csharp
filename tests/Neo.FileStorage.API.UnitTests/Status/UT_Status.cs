using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Status;

namespace Neo.FileStorage.API.UnitTests.Status
{
    [TestClass]
    public class UT_Status
    {
        [TestMethod]
        public void TestGlobalize()
        {
            var code1 = Success.Ok;
            Assert.AreEqual(0u, API.Status.Status.Globalize(code1));
            var code2 = CommonFail.Internal;
            Assert.AreEqual(1024u, API.Status.Status.Globalize(code2));
        }

        [TestMethod]
        public void TestLocalize()
        {
            Assert.AreEqual(Success.Ok, API.Status.Status.Localize(0));
            Assert.AreEqual(CommonFail.Internal, API.Status.Status.Localize(1024));
        }
    }
}
