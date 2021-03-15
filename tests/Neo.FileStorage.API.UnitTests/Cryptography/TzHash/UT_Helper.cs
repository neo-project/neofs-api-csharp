using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Neo.FileStorage.API.Cryptography.Tz.Helper;

namespace Neo.FileStorage.API.UnitTests.TestCryptography.Tz
{
    [TestClass]
    public class UT_Helper
    {
        [TestMethod]
        public void TestGetLeadingZeros()
        {
            ulong u1 = 1;
            int i = GetLeadingZeros(u1);
            Assert.AreEqual(63, i);
        }
    }
}
