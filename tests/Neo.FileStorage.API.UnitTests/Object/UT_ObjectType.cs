

using System;
using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Object;

namespace Neo.FileStorage.API.UnitTests.TestObject
{
    [TestClass]
    public class UT_ObjectType
    {
        enum TestType
        {
            ONE = 1,
            TWO = 2
        }

        [TestMethod]
        public void TestParse()
        {
            var t = "Regular";
            ObjectType type = Enum.Parse<ObjectType>(t);
            Assert.AreEqual(ObjectType.Regular, type);

            type = ObjectType.Regular;
            Console.WriteLine(type);

            var tt = TestType.ONE;
            Console.WriteLine(tt);
        }
    }
}
