using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Netmap;
using Neo.FileStorage.API.Refs;
using Newtonsoft.Json.Linq;

namespace Neo.FileStorage.API.UnitTests.TestNetmap
{
    [TestClass]
    public class UT_NetMap
    {
        [TestMethod]
        public void TestFlattenNodes()
        {
            var ns1 = new List<Node> { Helper.GenerateTestNode(0, ("Raing", "1")) };
            var ns2 = new List<Node> { Helper.GenerateTestNode(0, ("Raing", "2")) };
            List<List<Node>> list = new();
            list.Add(ns1);
            list.Add(ns2);
            Assert.AreEqual(2, list.Flatten().Count);
        }

        private string Indexes(List<Node> nodes)
        {
            string str = "{";
            for (int i = 0; i < nodes.Count; i++)
            {
                if (0 < i) str += ", ";
                str += nodes[i].Index.ToString();
            }
            str += "}";
            return str;
        }

        private string Indexes(List<List<Node>> nodes)
        {
            string str = "{";
            for (int i = 0; i < nodes.Count; i++)
            {
                if (0 < i) str += ", ";
                str += Indexes(nodes[i]);
            }
            str += "}";
            return str;
        }

        [TestMethod]
        public void TestSimulation()
        {
            var nodes_json_str = "[{\"id\":17245477228755262,\"capacity\":0,\"index\":0,\"price\":22,\"nodeinfo\":{\"publicKey\":\"Aiu0BBxQ1gf/hx3sfkzXd4OI4OpoSdhMy9mqjzLhaoEx\",\"addresses\":[\"/dns4/s01.neofs.devenv/tcp/8080\"],\"attributes\":[{\"key\":\"Capacity\",\"value\":\"0\"},{\"key\":\"Continent\",\"value\":\"Asia\"},{\"key\":\"Country\",\"value\":\"Russia\"},{\"key\":\"CountryCode\",\"value\":\"RU\"},{\"key\":\"Location\",\"value\":\"Mishkino\"},{\"key\":\"Price\",\"value\":\"22\"},{\"key\":\"UN-LOCODE\",\"value\":\"RU MSK\"}],\"state\":\"ONLINE\"}},{\"id\":14040999376522263000,\"capacity\":0,\"index\":1,\"price\":44,\"nodeinfo\":{\"publicKey\":\"A4yGKVnla0PiD3kYfE/p4Lx8jGbBYD5s8Ox/h6trCNw1\",\"addresses\":[\"/dns4/s04.neofs.devenv/tcp/8080\"],\"attributes\":[{\"key\":\"Capacity\",\"value\":\"0\"},{\"key\":\"Continent\",\"value\":\"Europe\"},{\"key\":\"Country\",\"value\":\"Finland\"},{\"key\":\"CountryCode\",\"value\":\"FI\"},{\"key\":\"Location\",\"value\":\"Helsinki (Helsingfors)\"},{\"key\":\"Price\",\"value\":\"44\"},{\"key\":\"SubDiv\",\"value\":\"Uusimaa\"},{\"key\":\"SubDivCode\",\"value\":\"18\"},{\"key\":\"UN-LOCODE\",\"value\":\"FI HEL\"}],\"state\":\"ONLINE\"}},{\"id\":1508609683428895200,\"capacity\":0,\"index\":2,\"price\":33,\"nodeinfo\":{\"publicKey\":\"A/9ltq55E0pNzp0NOdOFHpurTul6v4boHhxbvFDNKCau\",\"addresses\":[\"/dns4/s02.neofs.devenv/tcp/8080\"],\"attributes\":[{\"key\":\"Capacity\",\"value\":\"0\"},{\"key\":\"Continent\",\"value\":\"Europe\"},{\"key\":\"Country\",\"value\":\"Russia\"},{\"key\":\"CountryCode\",\"value\":\"RU\"},{\"key\":\"Location\",\"value\":\"Saint Petersburg (ex Leningrad)\"},{\"key\":\"Price\",\"value\":\"33\"},{\"key\":\"SubDiv\",\"value\":\"Sankt-Peterburg\"},{\"key\":\"SubDivCode\",\"value\":\"SPE\"},{\"key\":\"UN-LOCODE\",\"value\":\"RU LED\"}],\"state\":\"ONLINE\"}},{\"id\":11537578107108880000,\"capacity\":0,\"index\":3,\"price\":11,\"nodeinfo\":{\"publicKey\":\"AqySDNffC2GyiQcua5RuLaThoxuascYhu0deMPpKsQLD\",\"addresses\":[\"/dns4/s03.neofs.devenv/tcp/8080\"],\"attributes\":[{\"key\":\"Capacity\",\"value\":\"0\"},{\"key\":\"Continent\",\"value\":\"Europe\"},{\"key\":\"Country\",\"value\":\"Sweden\"},{\"key\":\"CountryCode\",\"value\":\"SE\"},{\"key\":\"Location\",\"value\":\"Stockholm\"},{\"key\":\"Price\",\"value\":\"11\"},{\"key\":\"SubDiv\",\"value\":\"Stockholms l�n\"},{\"key\":\"SubDivCode\",\"value\":\"AB\"},{\"key\":\"UN-LOCODE\",\"value\":\"SE STO\"}],\"state\":\"ONLINE\"}}]";
            var cid = ContainerID.FromString("AaCymCBPpMYeagJcBAxcMHKRZ9GEZYHDfLKsV1FwqAma");
            var oid1 = ObjectID.FromString("GMBa69wUJEgfoHnu37MKwudeYsemxHD38ynNsyuAZv2M");
            PlacementPolicy policy = new(0, new Replica[] { new(2, "SPB") }, new Selector[] { new("SPB", "City", Clause.Unspecified, 1, "*") }, null);
            var json = JArray.Parse(nodes_json_str);
            var nodes = (json).Select((p, i) => new Node(i, NodeInfo.Parser.ParseJson(p["nodeinfo"].ToString()))).ToList();
            NetMap nm = new(nodes);
            foreach (var n in nm.Nodes)
                Console.WriteLine($"{n.Index}, {n.Addresses[0]}");
            var container_nodes = nm.GetContainerNodes(policy, cid.Value.ToByteArray());
            Assert.AreEqual(1, container_nodes.Count);
            Assert.AreEqual(3, container_nodes.Flatten().Count);
            var indexes = Indexes(container_nodes);
            Console.WriteLine(indexes);
            var nodes1 = nm.GetPlacementVectors(container_nodes, oid1.Value.ToByteArray());
            Assert.AreEqual(1, nodes1.Count);
            var nodes1f = nodes1.Flatten();
            Assert.AreEqual(3, nodes1f.Count);
            indexes = Indexes(nodes1f);
            Console.WriteLine(indexes);
        }
    }
}
