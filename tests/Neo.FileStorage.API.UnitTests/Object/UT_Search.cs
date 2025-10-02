// Copyright (C) 2015-2025 The Neo Project.
//
// UT_Search.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Object;
using Neo.FileStorage.API.Refs;
using static Neo.FileStorage.API.Object.SearchRequest.Types.Body.Types;

namespace Neo.FileStorage.API.UnitTests.TestObject
{
    [TestClass]
    public class UT_Search
    {
        [TestMethod]
        public void TestFilter()
        {
            var sf = new SearchFilters();
            sf.AddFilter("header", "value", MatchType.StringEqual);
            Assert.AreEqual(1, sf.Filters.Length);
        }

        [TestMethod]
        public void TestAddRootFilters()
        {
            var sf = new SearchFilters();
            sf.AddRootFilter();
            var f = sf.Filters[0];

            Assert.AreEqual(MatchType.Unspecified, f.MatchType);
            Assert.AreEqual(Filter.FilterPropertyRoot, f.Key);
            Assert.AreEqual("", f.Value);
        }

        [TestMethod]
        public void TestAddPhyFilters()
        {
            var sf = new SearchFilters();
            sf.AddPhyFilter();
            var f = sf.Filters[0];

            Assert.AreEqual(MatchType.Unspecified, f.MatchType);
            Assert.AreEqual(Filter.FilterPropertyPhy, f.Key);
            Assert.AreEqual("", f.Value);
        }

        [TestMethod]
        public void TestAddParentIDFilters()
        {
            var sf = new SearchFilters();
            var oid = ObjectID.FromString("vWt34r4ddnq61jcPec4rVaXHg7Y7GiEYFmcTB2Qwhtx");
            sf.AddParentIDFilter(MatchType.StringEqual, oid);

            Assert.AreEqual(1, sf.Filters.Length);
            var f = sf.Filters[0];

            Assert.AreEqual(MatchType.StringEqual, f.MatchType);
            Assert.AreEqual(Filter.FilterHeaderParent, f.Key);
            Assert.AreEqual("vWt34r4ddnq61jcPec4rVaXHg7Y7GiEYFmcTB2Qwhtx", f.Value);
        }

        [TestMethod]
        public void TestAddObjectIDFilters()
        {
            var sf = new SearchFilters();
            var oid = ObjectID.FromString("vWt34r4ddnq61jcPec4rVaXHg7Y7GiEYFmcTB2Qwhtx");
            sf.AddObjectIDFilter(MatchType.StringEqual, oid);

            Assert.AreEqual(1, sf.Filters.Length);
            var f = sf.Filters[0];

            Assert.AreEqual(MatchType.StringEqual, f.MatchType);
            Assert.AreEqual(Filter.FilterHeaderObjectID, f.Key);
            Assert.AreEqual("vWt34r4ddnq61jcPec4rVaXHg7Y7GiEYFmcTB2Qwhtx", f.Value);
        }

        [TestMethod]
        public void TestAddSplitIDFilters()
        {
            var sf = new SearchFilters();
            var sid = new SplitID();
            sid.Parse("5dee2659-583f-492f-9ae1-2f5766ccab5c");
            sf.AddSplitIDFilter(MatchType.StringEqual, sid);
            Assert.AreEqual(1, sf.Filters.Length);
            var f = sf.Filters[0];

            Assert.AreEqual(MatchType.StringEqual, f.MatchType);
            Assert.AreEqual(Filter.FilterHeaderSplitID, f.Key);
            Assert.AreEqual("5dee2659-583f-492f-9ae1-2f5766ccab5c", f.Value);
        }

        [TestMethod]
        public void TestSearchRequestVerify()
        {
            var request = SearchRequest.Parser.ParseJson("{ \"body\": { \"containerId\": { \"value\": \"CQ+oCbU4N1ALayRO0r9zraZKVYI95mSgASONBzQVLgk=\" }, \"version\": 1, \"filters\": [ { \"matchType\": \"STRING_EQUAL\", \"key\": \"$Object:objectType\", \"value\": \"STORAGE_GROUP\" } ] }, \"metaHeader\": { \"ttl\": 1, \"origin\": { \"version\": { \"major\": 2, \"minor\": 9 }, \"ttl\": 2 } }, \"verifyHeader\": { \"metaSignature\": { \"key\": \"A/9ltq55E0pNzp0NOdOFHpurTul6v4boHhxbvFDNKCau\", \"signature\": \"BEm1TFZM1pPAFWjbOe+MFSB7NV/IzcIsuuac7XvnUYCDD4iyo/ehkwbBwA0Yxo0rZP4XQKPwmkg7vsrHyOU3ycc=\" }, \"originSignature\": { \"key\": \"A/9ltq55E0pNzp0NOdOFHpurTul6v4boHhxbvFDNKCau\", \"signature\": \"BAOV6M+ztiH6fkUhuu1xfU355ONJojUpMP0C0fv6Pd9GVAYsyHj6eG7qZ2uakHi9FPbwJgJ+jXn+oQgeyvAVeXc=\" }, \"origin\": { \"bodySignature\": { \"key\": \"ArNiK/QBe9/jF8WK7V9MdT8ga324lgRvp9d0u8S/f43C\", \"signature\": \"BPN/vPpiXeRtf1lhwdunK3f9MyzP6OYBqbGu0sRKEDTtG86tajYk9BBltbpMRBoyRvBiFdvAtASZygneu9LbY7E=\" }, \"metaSignature\": { \"key\": \"ArNiK/QBe9/jF8WK7V9MdT8ga324lgRvp9d0u8S/f43C\", \"signature\": \"BNzJEJr8iD7q+nfdMU4n8BUCQgZQuURryArXKHi5NKvoyryEOrZ7LjvkXCisk8/aWZhaDhLoDjW6GRB4PGNM+KM=\" }, \"originSignature\": { \"key\": \"ArNiK/QBe9/jF8WK7V9MdT8ga324lgRvp9d0u8S/f43C\", \"signature\": \"BEUV+T4/Xkc2WoBj1iGr9YQo5+n6oQQcMKTj1WhsEHdZ3Us6cRypDsESXhjCiD9JmoFc7Gvst+3eXXle0LKlTI8=\" } } } }");
            Assert.IsTrue(request.Verify());
        }
    }
}
