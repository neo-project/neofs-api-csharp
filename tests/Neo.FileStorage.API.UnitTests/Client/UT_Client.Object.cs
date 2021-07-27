using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.Cryptography;
using Neo.FileStorage.API.Client;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Cryptography.Tz;
using Neo.FileStorage.API.Object;
using Neo.FileStorage.API.Refs;
using V2Object = Neo.FileStorage.API.Object.Object;

namespace Neo.FileStorage.API.UnitTests.FSClient
{
    public partial class UT_Client
    {
        [TestMethod]
        public void TestObjectPut()
        {
            var rand = new Random();
            var payload = new byte[1024];
            rand.NextBytes(payload);
            var obj1 = new V2Object
            {
                Header = new Header
                {
                    OwnerId = OwnerID.FromScriptHash(key.PublicKey().PublicKeyToScriptHash()),
                    ContainerId = cid,
                },
                Payload = ByteString.CopyFrom(payload),
            };
            var obj2 = new V2Object
            {
                Header = new Header
                {
                    OwnerId = OwnerID.FromScriptHash(key.PublicKey().PublicKeyToScriptHash()),
                    ContainerId = cid,
                },
                Payload = ByteString.CopyFrom(payload),
            };
            using (var client1 = new Client.Client(key, host))
            {
                Session.SessionToken session;
                var source1 = new CancellationTokenSource();
                source1.CancelAfter(TimeSpan.FromMinutes(1));
                session = client1.CreateSession(ulong.MaxValue, context: source1.Token).Result;
                source1.Cancel();
                var source2 = new CancellationTokenSource();
                source2.CancelAfter(TimeSpan.FromMinutes(1));
                var o = client1.PutObject(obj1, new CallOptions { Ttl = 2, Session = session }, source2.Token).Result;
                Console.WriteLine(o.ToBase58String());
                Assert.AreNotEqual("", o.ToBase58String());
            }
            using (var client2 = new Client.Client(key, host))
            {
                Session.SessionToken session;
                var source1 = new CancellationTokenSource();
                source1.CancelAfter(TimeSpan.FromMinutes(1));
                session = client2.CreateSession(ulong.MaxValue, context: source1.Token).Result;
                source1.Cancel();
                var source2 = new CancellationTokenSource();
                source2.CancelAfter(TimeSpan.FromMinutes(1));
                var o = client2.PutObject(obj2, new CallOptions { Ttl = 2, Session = session }, source2.Token).Result;
                Console.WriteLine(o.ToBase58String());
                Assert.AreNotEqual("", o.ToBase58String());
            }
        }

        [TestMethod]
        public void TestObjectPutStorageGroup()
        {
            List<ObjectID> oids = new() { ObjectID.FromBase58String("7Q7fPKESmRJ1VGHKcB2pB4JWVebsQzrJypwQiNLU1ekv"), ObjectID.FromBase58String("HwfVt5i9ucjPUhRpHyxamnfTvhKtTUysCZWXcJ6YZsZ4") };
            using var client = new Client.Client(key, host);
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
                    OwnerId = OwnerID.FromScriptHash(key.PublicKey().PublicKeyToScriptHash()),
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
            var address = new Address(cid, oid);
            using var client = new Client.Client(key, host);
            var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.GetObject(address, false, new CallOptions { Ttl = 2 }, source.Token).Result;
            Assert.AreEqual(oid, o.ObjectId);
            Console.WriteLine(o.ToJson().ToString());
        }

        [TestMethod]
        public void TestObjectGetWithoutOptions()
        {
            var address = new Address(cid, oid);
            using var client = new Client.Client(key, host);
            var source = new CancellationTokenSource();
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
            Assert.AreEqual(address, o);
        }

        [TestMethod]
        public void TestObjectHeaderGet()
        {
            var address = new Address(cid, oid);
            using var client = new Client.Client(key, host);
            var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.GetObjectHeader(address, false, false, new CallOptions { Ttl = 2 }, source.Token).Result;
            Assert.AreEqual(oid, o.ObjectId);
        }

        [TestMethod]
        public void TestObjectGetRange()
        {
            var address = new Address(cid, oid);
            using var client = new Client.Client(key, host);
            var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.GetObjectPayloadRangeData(address, new Object.Range { Offset = 0, Length = 3 }, false, new CallOptions { Ttl = 2 }, source.Token).Result;
            Assert.AreEqual("hel", Encoding.ASCII.GetString(o));
        }

        [TestMethod]
        public void TestObjectGetRangeHash()
        {
            var address = new Address(cid, oid);
            using var client = new Client.Client(key, host);
            var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.GetObjectPayloadRangeHash(address, new List<Object.Range> { new Object.Range { Offset = 0, Length = 3 } }, ChecksumType.Sha256, new byte[] { 0x00 }, new CallOptions { Ttl = 2 }, source.Token).Result;
            Assert.AreEqual(1, o.Count);
            Assert.AreEqual(Encoding.ASCII.GetBytes("hello")[..3].Sha256().ToHexString(), o[0].ToHexString());
        }

        [TestMethod]
        public void TestObjectSearch()
        {
            using var client = new Client.Client(key, host);
            var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var filter = new SearchFilters();
            filter.AddTypeFilter(MatchType.StringEqual, ObjectType.StorageGroup);
            var o = client.SearchObject(cid, filter, new CallOptions { Ttl = 2 }, source.Token).Result;
            o.ForEach(p => Console.WriteLine(p.ToBase58String()));
            Assert.IsTrue(o.Select(p => p.ToBase58String()).ToList().Contains("Cci6sUPwwPtx3LXyCRaYHroesedP98Vctu8d8T52vFKX"));
        }

        [TestMethod]
        public void TestClient()
        {
            var key = "7310c4da083264666cc3567d5cb4a5b060ca34fb68959de58bd04959f3cbc6b2".HexToBytes().LoadPrivateKey();
            _ = new Client.Client(key, "127.0.0.1:8080");
        }
    }
}
