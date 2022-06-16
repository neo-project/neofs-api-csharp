using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Protobuf.WellKnownTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Netmap;
using Newtonsoft.Json;

namespace Neo.FileStorage.API.UnitTests.TestNetmap
{
    [TestClass]
    public class TestNetmap
    {
        private const string jsonDir = "./json_tests";
        private List<TestContext> tests = new();

        private class Placement
        {
            public byte[] Pivot = null;
            public int[][] Result = null;
        }

        private class TestCase
        {
            public string Name = "";
            public PlacementPolicy Policy = null;
            public byte[] Pivot = null;
            public int[][] Result = null;
            public string Error = "";
            public Placement Placement = null;

            public void Test(TestContext context)
            {
                try
                {
                    var nss = context.NetMap.GetContainerNodes(Policy, Pivot);
                    if (Result is not null)
                    {
                        CheckResult(Result, nss);
                    }
                    if (Placement is not null)
                    {
                        nss = context.NetMap.GetPlacementVectors(nss, Placement.Pivot);
                        CheckResult(Placement.Result, nss);
                    }
                }
                catch (Exception e)
                {
                    if (Error is not null && Error != "") Assert.IsTrue(e.Message.Contains(Error));
                }
            }

            private void CheckResult(int[][] result, List<List<Node>> nss)
            {
                Assert.AreEqual(result.Length, nss.Count);
                for (int i = 0; i < result.Length; i++)
                {
                    Assert.AreEqual(result[i].Length, nss[i].Count);
                    for (int j = 0; j < result[i].Length; j++)
                    {
                        Assert.AreEqual(result[i][j], nss[i][j].Index);
                    }
                }
            }
        }

        private class TAttribute
        {
            public string Key = "";
            public string Value = "";
        }

        private class TestNode
        {
            public List<TAttribute> Attributes = null;

            public Node ToNode(int i)
            {
                NodeInfo ni = new();
                foreach (var attr in Attributes)
                {
                    ni.Attributes.Add(new NodeInfo.Types.Attribute() { Key = attr.Key, Value = attr.Value });
                }
                return new Node(i, ni);
            }
        }

        private class TestContext
        {
            public string Name = "";
            public List<TestNode> Nodes = null;
            public Dictionary<string, TestCase> Tests = null;
            public NetMap NetMap = null;

            public void Test()
            {
                if (NetMap is null)
                    NetMap = new(Nodes.Select((n, i) => n.ToNode(i)).ToList());
                foreach (var tc in Tests)
                {
                    tc.Value.Test(this);
                }
            }


        }

        [TestInitialize]
        public void LoadTest()
        {
            foreach (var path in Directory.GetFiles(jsonDir))
            {
                if (File.Exists(path))
                {
                    var context = JsonConvert.DeserializeObject<TestContext>(File.ReadAllText(path), new EnumJsonConverter());
                    tests.Add(context);
                }
            }
        }

        [TestMethod]
        public void Test()
        {
            foreach (var test in tests)
            {
                test.Test();
            }
        }
    }
}
