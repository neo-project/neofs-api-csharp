// Copyright (C) 2015-2025 The Neo Project.
//
// UT_Encode.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Policy;
using static Neo.FileStorage.API.Policy.Helper;

namespace Neo.FileStorage.API.UnitTests.Policy
{
    [TestClass]
    public class UT_Encode
    {
        [TestMethod]
        public void TestEncode()
        {
            var testCases = new string[]
            {
                @"REP 1 IN X
CBF 1
SELECT 2 IN SAME Location FROM * AS X",
                @"REP 1
SELECT 2 IN City FROM Good
FILTER Country EQ RU AS FromRU
FILTER @FromRU AND Rating GT 7 AS Good",
                @"REP 7 IN SPB
SELECT 1 IN City FROM SPBSSD AS SPB
FILTER City EQ SPB AND SSD EQ true OR City EQ SPB AND Rating GE 5 AS SPBSSD"
            };

            foreach (var tc in testCases)
            {
                var pp = ParsePlacementPolicy(tc);
                var got = Encode.EncodePlacementPolicy(pp);
                var res = string.Join("\n", got);
                Assert.AreEqual(tc, res);
            }
        }


        [TestMethod]
        public void TestDecode()
        {
            string tc = @"REP 2 IN X CBF 2 SELECT 2 FROM F AS X FILTER UN-LOCODE EQ ""DE FRA"" AS F";
            var pp = ParsePlacementPolicy(tc);
            var got = Encode.EncodePlacementPolicy(pp);
            var res = string.Join(" ", got);
            Assert.AreEqual(tc, res);
        }
    }
}
