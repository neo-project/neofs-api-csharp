using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            public byte[] Pivot;
            public int[][] Result;
        }

        private class TestCase
        {
            public string Name;
            public PlacementPolicy Policy;
            public byte[] Pivot;
            public int[][] Result;
            public string Error;
            public Placement Placement;

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
                    if (Error != "") Assert.IsTrue(e.Message.Contains(Error));
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

        private class TestContext
        {
            public string Name;
            public Node[] Nodes;
            public TestCase[] Tests;
            public NetMap NetMap;

            public void Test()
            {
                if (NetMap is null)
                    NetMap = new(Nodes.ToList());
                foreach (var tc in Tests)
                {
                    tc.Test(this);
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
                    var context = JsonConvert.DeserializeObject<TestContext>(File.ReadAllText(path));
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
