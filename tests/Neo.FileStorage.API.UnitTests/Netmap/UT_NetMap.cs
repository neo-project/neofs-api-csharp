using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Netmap;
using Neo.FileStorage.API.Refs;
using Neo.IO.Json;

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

        [TestMethod]
        public void TestSimulation()
        {
            var nodes_json_str = "[{\"id\":17245477228755262,\"capacity\":0,\"index\":0,\"price\":22,\"nodeinfo\":{\"publicKey\":\"Aiu0BBxQ1gf/hx3sfkzXd4OI4OpoSdhMy9mqjzLhaoEx\",\"address\":\"/dns4/s01.neofs.devenv/tcp/8080\",\"attributes\":[{\"key\":\"Capacity\",\"value\":\"0\"},{\"key\":\"Continent\",\"value\":\"Asia\"},{\"key\":\"Country\",\"value\":\"Russia\"},{\"key\":\"CountryCode\",\"value\":\"RU\"},{\"key\":\"Location\",\"value\":\"Mishkino\"},{\"key\":\"Price\",\"value\":\"22\"},{\"key\":\"UN-LOCODE\",\"value\":\"RU MSK\"}],\"state\":\"Online\"}},{\"id\":14040999376522263000,\"capacity\":0,\"index\":1,\"price\":44,\"nodeinfo\":{\"publicKey\":\"A4yGKVnla0PiD3kYfE/p4Lx8jGbBYD5s8Ox/h6trCNw1\",\"address\":\"/dns4/s04.neofs.devenv/tcp/8080\",\"attributes\":[{\"key\":\"Capacity\",\"value\":\"0\"},{\"key\":\"Continent\",\"value\":\"Europe\"},{\"key\":\"Country\",\"value\":\"Finland\"},{\"key\":\"CountryCode\",\"value\":\"FI\"},{\"key\":\"Location\",\"value\":\"Helsinki (Helsingfors)\"},{\"key\":\"Price\",\"value\":\"44\"},{\"key\":\"SubDiv\",\"value\":\"Uusimaa\"},{\"key\":\"SubDivCode\",\"value\":\"18\"},{\"key\":\"UN-LOCODE\",\"value\":\"FI HEL\"}],\"state\":\"Online\"}},{\"id\":1508609683428895200,\"capacity\":0,\"index\":2,\"price\":33,\"nodeinfo\":{\"publicKey\":\"A/9ltq55E0pNzp0NOdOFHpurTul6v4boHhxbvFDNKCau\",\"address\":\"/dns4/s02.neofs.devenv/tcp/8080\",\"attributes\":[{\"key\":\"Capacity\",\"value\":\"0\"},{\"key\":\"Continent\",\"value\":\"Europe\"},{\"key\":\"Country\",\"value\":\"Russia\"},{\"key\":\"CountryCode\",\"value\":\"RU\"},{\"key\":\"Location\",\"value\":\"Saint Petersburg (ex Leningrad)\"},{\"key\":\"Price\",\"value\":\"33\"},{\"key\":\"SubDiv\",\"value\":\"Sankt-Peterburg\"},{\"key\":\"SubDivCode\",\"value\":\"SPE\"},{\"key\":\"UN-LOCODE\",\"value\":\"RU LED\"}],\"state\":\"Online\"}},{\"id\":11537578107108880000,\"capacity\":0,\"index\":3,\"price\":11,\"nodeinfo\":{\"publicKey\":\"AqySDNffC2GyiQcua5RuLaThoxuascYhu0deMPpKsQLD\",\"address\":\"/dns4/s03.neofs.devenv/tcp/8080\",\"attributes\":[{\"key\":\"Capacity\",\"value\":\"0\"},{\"key\":\"Continent\",\"value\":\"Europe\"},{\"key\":\"Country\",\"value\":\"Sweden\"},{\"key\":\"CountryCode\",\"value\":\"SE\"},{\"key\":\"Location\",\"value\":\"Stockholm\"},{\"key\":\"Price\",\"value\":\"11\"},{\"key\":\"SubDiv\",\"value\":\"Stockholms l�n\"},{\"key\":\"SubDivCode\",\"value\":\"AB\"},{\"key\":\"UN-LOCODE\",\"value\":\"SE STO\"}],\"state\":\"Online\"}}]";
            var cid = ContainerID.FromString("EG9aHdMgGyKpjerpoKWQsggTu5VYkeMY4C46UVBsniEn");
            var oid1 = ObjectID.FromString("3tCBy81Ke4pNxC1ptMTUWU7V1aBr7zBsz4xkj6nTzNqJ");
            var oid2 = ObjectID.FromString("DR6QEovugA1MGDk4pGz5BTMXhsq5q3Qeyy6aeCtA43sk");
            PlacementPolicy policy = new(0, new Replica[] { new(2, "SPB") }, new Selector[] { new("SPB", "City", Clause.Unspecified, 1, "*") }, null);
            var json = JObject.Parse(nodes_json_str);
            var nodes = ((JArray)json).Select(p => Node.FromJson(p)).ToList();
            NetMap nm = new(nodes);
            var container_nodes = nm.GetContainerNodes(policy, cid.Value.ToByteArray());
            Assert.AreEqual(3, container_nodes.Flatten().Count);
            var nodes1 = nm.GetPlacementVectors(container_nodes, oid1.Value.ToByteArray()).Flatten();
            Assert.AreEqual(3, nodes1.Count);
            Console.WriteLine($"{nodes1[0].NetworkAddresses[0]}, {nodes1[1].NetworkAddresses[0]}");
            var nodes2 = nm.GetPlacementVectors(container_nodes, oid2.Value.ToByteArray()).Flatten();
            Assert.AreEqual(3, nodes2.Count);
            Console.WriteLine($"{nodes1[0].NetworkAddresses[0]}, {nodes1[1].NetworkAddresses[0]}");
        }
    }
}