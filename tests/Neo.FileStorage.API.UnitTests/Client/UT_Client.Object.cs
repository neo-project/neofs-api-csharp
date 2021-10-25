using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Client;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Cryptography.Tz;
using Neo.FileStorage.API.Object;
using Neo.FileStorage.API.Refs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using V2Object = Neo.FileStorage.API.Object.Object;

namespace Neo.FileStorage.API.UnitTests.FSClient
{
    public partial class UT_Client
    {
        [TestMethod]
        public void TestObjectPutFull()
        {
            var obj = RandomFullObject(1024);
            Console.WriteLine("created object, id=" + obj.ObjectId.String());
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            var o = client.PutObject(obj, new CallOptions { Ttl = 1 }, source.Token).Result;
            Console.WriteLine("get object, id=" + o.String());
            Assert.AreNotEqual("", o.String());
        }

        [TestMethod]
        public void TestObjectPut()
        {
            var rand = new Random();
            var payload = new byte[67118864];
            rand.NextBytes(payload);
            var obj = new V2Object
            {
                Header = new Header
                {
                    OwnerId = OwnerID.FromScriptHash(key.PublicKey().PublicKeyToScriptHash()),
                    ContainerId = cid,
                },
                Payload = ByteString.CopyFrom(payload),
            };
            obj.Attributes.Add(new Header.Types.Attribute
            {
                Key = "category",
                Value = "test"
            });
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(10));
            var session = client.CreateSession(20).Result;
            var o = client.PutObject(obj, new CallOptions { Ttl = 1, Session = session }, source.Token).Result;
            Console.WriteLine(o.String());
            Assert.AreNotEqual("", o.String());
        }

        [TestMethod]
        public void TestObjectPutStorageGroup()
        {
            List<ObjectID> oids = new() { ObjectID.FromString("7Q7fPKESmRJ1VGHKcB2pB4JWVebsQzrJypwQiNLU1ekv"), ObjectID.FromString("HwfVt5i9ucjPUhRpHyxamnfTvhKtTUysCZWXcJ6YZsZ4") };
            using var client = new Client.Client(key, host);
            byte[] tzh = null;
            ulong size = 0;
            foreach (var oid in oids)
            {
                var address = new Address(cid, oid);
                using var source = new CancellationTokenSource();
                source.CancelAfter(TimeSpan.FromMinutes(1));
                var oo = client.GetObjectHeader(address, false, false, new CallOptions { Ttl = 2 }, source.Token).Result;
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
                    OwnerId = OwnerID.FromScriptHash(key.PublicKey().PublicKeyToScriptHash()),
                    ContainerId = cid,
                    ObjectType = ObjectType.StorageGroup,
                },
                Payload = ByteString.CopyFrom(sg.ToByteArray()),
            };
            using var source1 = new CancellationTokenSource();
            source1.CancelAfter(TimeSpan.FromMinutes(1));
            var session = client.CreateSession(ulong.MaxValue, context: source1.Token).Result;
            source1.Cancel();
            using var source2 = new CancellationTokenSource();
            source2.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.PutObject(obj, new CallOptions { Ttl = 2, Session = session }, source2.Token).Result;
            Console.WriteLine(o.String());
            Assert.AreNotEqual("", o.String());
        }

        [TestMethod]
        public void TestObjectGet()
        {
            var address = new Address(cid, oid);
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromSeconds(10));
            var o = client.GetObject(address, false, new CallOptions { Ttl = 1 }, source.Token).Result;
            Console.WriteLine(o.Header.ToString());
            Console.WriteLine(o.Payload.Length);
            Assert.AreEqual(oid, o.ObjectId);
        }

        [TestMethod]
        public void TestObjectGetWithStream()
        {
            var file = File.Create("objectgetstream.data");
            var address = new Address(cid, oid);
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromSeconds(10));
            client.GetObject(address, file, false, new CallOptions { Ttl = 1 }, source.Token).Wait();
            file.Flush();
            Console.WriteLine(file.Length);
        }

        [TestMethod]
        public void TestObjectGetWithoutOptions()
        {
            var address = new Address(cid, oid);
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.GetObject(address, context: source.Token).Result;
            Assert.AreEqual(oid, o.ObjectId);
        }

        [TestMethod]
        public void TestObjectDelete()
        {
            var address = new Address(cid, oid);
            using var client = new Client.Client(key, host);
            var source1 = new CancellationTokenSource();
            source1.CancelAfter(TimeSpan.FromMinutes(1));
            var session = client.CreateSession(ulong.MaxValue, context: source1.Token).Result;
            source1.Cancel();
            var source2 = new CancellationTokenSource();
            source2.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.DeleteObject(address, new CallOptions { Ttl = 2, Session = session }, source2.Token).Result;
            Assert.IsNotNull(o);
            Assert.AreEqual(oid.String(), o.ObjectId.String());
        }

        [TestMethod]
        public void TestObjectHeaderGet()
        {
            var address = new Address(cid, oid);
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.GetObjectHeader(address, false, false, new CallOptions { Ttl = 1 }, source.Token).Result;
            Console.WriteLine(o);
            Assert.AreEqual(oid, o.ObjectId);
        }

        [TestMethod]
        public void TestObjectGetRange()
        {
            var address = new Address(cid, oid);
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.GetObjectPayloadRangeData(address, new Object.Range { Offset = 0, Length = 100 }, false, new CallOptions { Ttl = 1 }, source.Token).Result;
            Console.WriteLine(o.Length);
            Console.WriteLine(o.ToHexString());
        }

        [TestMethod]
        public void TestObjectGetRangeHash()
        {
            var address = new Address(cid, oid);
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.GetObjectPayloadRangeHash(address, new List<Object.Range> { new Object.Range { Offset = 0, Length = 100 } }, ChecksumType.Sha256, new byte[] { }, new CallOptions { Ttl = 2 }, source.Token).Result;
            Console.WriteLine($"{o.Count} {string.Join(", ", o.Select(p => p.ToHexString()))}");
        }

        [TestMethod]
        public void TestObjectSearch()
        {
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var filter = new SearchFilters();
            filter.AddTypeFilter(Object.MatchType.StringEqual, ObjectType.Regular);
            var o = client.SearchObject(cid, filter, new CallOptions { Ttl = 2 }, source.Token).Result;
            Console.WriteLine($"{o.Count} {string.Join(", ", o.Select(p => p.String()))}");
            Assert.IsTrue(o.Contains(oid));
        }
    }
}
