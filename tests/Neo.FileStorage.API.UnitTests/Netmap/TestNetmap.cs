using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Netmap;
using Neo.IO.Json;

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

            public static Placement FromJson(JObject json)
            {
                if (json is null) return null;
                return new()
                {
                    Pivot = json["pivot"] is not null ? Convert.FromBase64String(json["pivot"].GetString()) : null,
                    Result = ((JArray)json["result"])?.Select(p => ((JArray)p).Select(q => int.Parse(q.AsString())).ToArray()).ToArray(),
                };
            }
        }

        private class TestCase
        {
            public string Name;
            public PlacementPolicy Policy;
            public byte[] Pivot;
            public int[][] Result;
            public string Error;
            public Placement Placement;

            public static TestCase FromJson(string name, JObject json)
            {
                return new()
                {
                    Name = name,
                    Policy = PlacementPolicy.Parser.ParseJson(json["policy"].ToString()),
                    Pivot = json["pivot"] is not null ? Convert.FromBase64String(json["pivot"].GetString()) : null,
                    Result = ((JArray)json["result"])?.Select(p => ((JArray)p).Select(q => int.Parse(q.AsString())).ToArray()).ToArray(),
                    Error = json["error"]?.GetString() ?? "",
                    Placement = Placement.FromJson(json["placement"])
                };
            }

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
            public List<Node> Nodes;
            public TestCase[] Cases;
            public NetMap NetMap;
            public static TestContext FromJson(JObject json)
            {
                return new()
                {
                    Name = json["name"].GetString(),
                    Nodes = ((JArray)json["nodes"]).Select((p, i) => new Node(i, NodeInfo.Parser.ParseJson(p.ToString()))).ToList(),
                    Cases = json["tests"].Properties.Select(p => TestCase.FromJson(p.Key, p.Value)).ToArray(),
                };
            }

            public void Test()
            {
                if (NetMap is null)
                    NetMap = new(Nodes);
                foreach (var tc in Cases)
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
                    JObject test = JObject.Parse(File.ReadAllText(path));
                    TestContext context = TestContext.FromJson(test);
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
