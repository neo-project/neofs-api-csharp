using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.Cryptography;
using Neo.FileStorage.API.Client;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Cryptography.Tz;
using Neo.FileStorage.API.Object;
using Neo.FileStorage.API.Refs;
using Neo.FileStorage.API.StorageGroup;
using V2Object = Neo.FileStorage.API.Object.Object;

namespace Neo.FileStorage.API.UnitTests.FSClient
{
    [TestClass]
    public class UT_Object
    {
        [TestMethod]
        public void TestObjectPut()
        {
            var host = "http://st1.storage.fs.neo.org:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var cid = ContainerID.FromBase58String("ETptK9H8wd5i3zt3JQmuArupPAGbz24YnCWA9Cs91rs6");
            var rand = new Random();
            var payload = new byte[1024];
            rand.NextBytes(payload);
            var obj = new V2Object
            {
                Header = new Header
                {
                    OwnerId = key.ToOwnerID(),
                    ContainerId = cid,
                },
                Payload = ByteString.CopyFrom(payload),
            };
            var client = new Client.Client(key, host);
            var source1 = new CancellationTokenSource();
            source1.CancelAfter(TimeSpan.FromMinutes(1));
            var session = client.CreateSession(ulong.MaxValue, context: source1.Token).Result;
            source1.Cancel();
            var source2 = new CancellationTokenSource();
            source2.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.PutObject(obj, new CallOptions { Ttl = 2, Session = session }, source2.Token).Result;
            Console.WriteLine(o.ToBase58String());
            Assert.AreNotEqual("", o.ToBase58String());
        }

        [TestMethod]
        public void TestObjectPutStorageGroup()
        {
            var host = "http://st1.storage.fs.neo.org:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var cid = ContainerID.FromBase58String("ETptK9H8wd5i3zt3JQmuArupPAGbz24YnCWA9Cs91rs6");

            List<ObjectID> oids = new() { ObjectID.FromBase58String("7A27ou91pzJinEbgC1XCA4Cao4ragF82weCT6jpC3dc2"), ObjectID.FromBase58String("9kkAQXcSqvjWo67GX4JdsmTFPKg8NVMuD1RhBs6bg6di") };
            var client = new Client.Client(key, host);
            byte[] tzh = null;
            ulong size = 0;
            foreach (var oid in oids)
            {
                var address = new Address(cid, oid);
                var source = new CancellationTokenSource();
                source.CancelAfter(TimeSpan.FromMinutes(1));
                var oo = client.GetObject(address, false, new CallOptions { Ttl = 2 }, source.Token).Result;
                if (tzh is null)
                    tzh = oo.PayloadHomomorphicHash.Sum.ToByteArray();
                else
                    tzh = TzHash.Concat(new() { tzh, oo.PayloadHomomorphicHash.Sum.ToByteArray() });
                size += oo.PayloadSize;
            }
            var epoch = client.Epoch().Result;
            StorageGroup.StorageGroup sg = new()
            {
                ValidationDataSize = size,
                ValidationHash = new()
                {
                    Type = ChecksumType.Tz,
                    Sum = ByteString.CopyFrom(tzh)
                },
                ExpirationEpoch = epoch + 100,
            };
            sg.Members.AddRange(oids);
            var obj = new V2Object
            {
                Header = new Header
                {
                    OwnerId = key.ToOwnerID(),
                    ContainerId = cid,
                    ObjectType = ObjectType.StorageGroup,
                },
                Payload = ByteString.CopyFrom(sg.ToByteArray()),
            };
            var source1 = new CancellationTokenSource();
            source1.CancelAfter(TimeSpan.FromMinutes(1));
            var session = client.CreateSession(ulong.MaxValue, context: source1.Token).Result;
            source1.Cancel();
            var source2 = new CancellationTokenSource();
            source2.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.PutObject(obj, new CallOptions { Ttl = 2, Session = session }, source2.Token).Result;
            Console.WriteLine(o.ToBase58String());
            Assert.AreNotEqual("", o.ToBase58String());
        }

        [TestMethod]
        public void TestObjectGet()
        {
            var host = "http://st2.storage.fs.neo.org:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var cid = ContainerID.FromBase58String("ETptK9H8wd5i3zt3JQmuArupPAGbz24YnCWA9Cs91rs6");
            var oid = ObjectID.FromBase58String("8y6hSCSueoD5or9gdRygFnCPqkyJErNuA2NPPxqptGxb");
            var address = new Address(cid, oid);
            var client = new Client.Client(key, host);
            var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.GetObject(address, false, new CallOptions { Ttl = 2 }, source.Token).Result;
            Assert.AreEqual(oid, o.ObjectId);
            Console.WriteLine(o.ToJson().ToString());
        }

        [TestMethod]
        public void TestObjectGetWithoutOptions()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var cid = ContainerID.FromBase58String("RuzuV3RDstuVtWoDzsTsuNFiakaaGGN24EbNSUFGaiQ");
            var oid = ObjectID.FromBase58String("6VLqsZAvYTRzt8yY4NvGweWfGmqBiAfQwd6novRNFYiG");
            var address = new Address(cid, oid);
            var client = new Client.Client(key, host);
            var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.GetObject(address, context: source.Token).Result;
            Assert.AreEqual(oid, o.ObjectId);
        }

        [TestMethod]
        public void TestObjectDelete()
        {
            var host = "http://st1.storage.fs.neo.org:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var cid = ContainerID.FromBase58String("7tJhXBvoCGdPonLUL4FwK5RT1oVczQJsU21xjHqMY7ec");
            var oid = ObjectID.FromBase58String("Y1VUWvCeW7HUecFNoeHR48Ev4NyU61uH1YRuDVTiK57");
            var address = new Address(cid, oid);
            var client = new Client.Client(key, host);
            var source1 = new CancellationTokenSource();
            source1.CancelAfter(TimeSpan.FromMinutes(1));
            var session = client.CreateSession(ulong.MaxValue, context: source1.Token).Result;
            source1.Cancel();
            var source2 = new CancellationTokenSource();
            source2.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.DeleteObject(address, new CallOptions { Ttl = 2, Session = session }, source2.Token).Result;
            Assert.AreEqual(address, o);
        }

        [TestMethod]
        public void TestObjectHeaderGet()
        {
            var host = "http://st2.storage.fs.neo.org:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var cid = ContainerID.FromBase58String("6pJtLUnGqDxE2EitZYLsDzsfTDVegD6BrRUn8QAFZWyt");
            var oid = ObjectID.FromBase58String("5Cyxb3wrHDw5pqY63hb5otCSsJ24ZfYmsA8NAjtho2gr");
            var address = new Address(cid, oid);
            var client = new Client.Client(key, host);
            var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.GetObjectHeader(address, false, false, new CallOptions { Ttl = 2 }, source.Token).Result;
            Assert.AreEqual(oid, o.ObjectId);
        }

        [TestMethod]
        public void TestObjectGetRange()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var cid = ContainerID.FromBase58String("Bun3sfMBpnjKc3Tx7SdwrMxyNi8ha8JT3dhuFGvYBRTz");
            var oid = ObjectID.FromBase58String("vWt34r4ddnq61jcPec4rVaXHg7Y7GiEYFmcTB2Qwhtx");
            var address = new Address(cid, oid);
            var client = new Client.Client(key, host);
            var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.GetObjectPayloadRangeData(address, new Object.Range { Offset = 0, Length = 3 }, false, new CallOptions { Ttl = 2 }, source.Token).Result;
            Assert.AreEqual("hel", Encoding.ASCII.GetString(o));
        }

        [TestMethod]
        public void TestObjectGetRangeHash()
        {
            var host = "http://st2.storage.fs.neo.org:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var cid = ContainerID.FromBase58String("Bun3sfMBpnjKc3Tx7SdwrMxyNi8ha8JT3dhuFGvYBRTz");
            var oid = ObjectID.FromBase58String("vWt34r4ddnq61jcPec4rVaXHg7Y7GiEYFmcTB2Qwhtx");
            var address = new Address(cid, oid);
            var client = new Client.Client(key, host);
            var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.GetObjectPayloadRangeHash(address, new List<Object.Range> { new Object.Range { Offset = 0, Length = 3 } }, ChecksumType.Sha256, new byte[] { 0x00 }, new CallOptions { Ttl = 2 }, source.Token).Result;
            Assert.AreEqual(1, o.Count);
            Assert.AreEqual(Encoding.ASCII.GetBytes("hello")[..3].Sha256().ToHexString(), o[0].ToHexString());
        }

        [TestMethod]
        public void TestObjectSearch()
        {
            var host = "http://st2.storage.fs.neo.org:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var cid = ContainerID.FromBase58String("ETptK9H8wd5i3zt3JQmuArupPAGbz24YnCWA9Cs91rs6");
            var client = new Client.Client(key, host);
            var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var filter = new SearchFilters();
            filter.AddTypeFilter(MatchType.StringEqual, ObjectType.StorageGroup);
            var o = client.SearchObject(cid, filter, new CallOptions { Ttl = 2 }, source.Token).Result;
            o.ForEach(p => Console.WriteLine(p.ToBase58String()));
            Assert.IsTrue(o.Select(p => p.ToBase58String()).ToList().Contains("8y6hSCSueoD5or9gdRygFnCPqkyJErNuA2NPPxqptGxb"));
        }

        [TestMethod]
        public void TestClient()
        {
            var key = "7310c4da083264666cc3567d5cb4a5b060ca34fb68959de58bd04959f3cbc6b2".HexToBytes().LoadPrivateKey();
            _ = new Client.Client(key, "127.0.0.1:8080");
        }
    }
}
