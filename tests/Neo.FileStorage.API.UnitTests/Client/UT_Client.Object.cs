using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void TestObjectPutFull()
        {
            var obj = RandomFullObject(40000);
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
            var payload = new byte[1024];
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
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var session = client.CreateSession(10).Result;
            var o = client.PutObject(obj, new CallOptions { Ttl = 2, Session = session }, source.Token).Result;
            Console.WriteLine(o.String());
            Assert.AreNotEqual("", o.String());
        }

        [TestMethod]
        public void TestObjectPutTwice()
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
                using (var source1 = new CancellationTokenSource())
                {
                    source1.CancelAfter(TimeSpan.FromMinutes(1));
                    session = client1.CreateSession(ulong.MaxValue, context: source1.Token).Result;
                    source1.Cancel();
                }
                using (var source2 = new CancellationTokenSource())
                {
                    source2.CancelAfter(TimeSpan.FromMinutes(1));
                    var o = client1.PutObject(obj1, new CallOptions { Ttl = 2, Session = session }, source2.Token).Result;
                    Console.WriteLine(o.String());
                    Assert.AreNotEqual("", o.String());
                }
            }
            using (var client2 = new Client.Client(key, host))
            {
                Session.SessionToken session;
                using (var source1 = new CancellationTokenSource())
                {
                    source1.CancelAfter(TimeSpan.FromMinutes(1));
                    session = client2.CreateSession(ulong.MaxValue, context: source1.Token).Result;
                    source1.Cancel();
                }
                using (var source2 = new CancellationTokenSource())
                {
                    source2.CancelAfter(TimeSpan.FromMinutes(1));
                    var o = client2.PutObject(obj1, new CallOptions { Ttl = 2, Session = session }, source2.Token).Result;
                    Console.WriteLine(o.String());
                    Assert.AreNotEqual("", o.String());
                }
            }
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
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.GetObject(address, false, new CallOptions { Ttl = 1 }, source.Token).Result;
            Console.WriteLine(o.Header.ToString());
            Console.WriteLine(o.Payload.Length);
            Assert.AreEqual(oid, o.ObjectId);
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
            ObjectID coid = ObjectID.FromString("8k7WGNFFjccbKQxxWFtRXhYqYQqzctKd8kyB4rKfrBza");
            var address = new Address(cid, coid);
            using var client = new Client.Client(key, host);
            var source1 = new CancellationTokenSource();
            source1.CancelAfter(TimeSpan.FromMinutes(1));
            var session = client.CreateSession(ulong.MaxValue, context: source1.Token).Result;
            source1.Cancel();
            var source2 = new CancellationTokenSource();
            source2.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.DeleteObject(address, new CallOptions { Ttl = 2, Session = session }, source2.Token).Result;
            Assert.AreEqual(coid.String(), o.ObjectId.String());
        }

        [TestMethod]
        public void TestObjectHeaderGet()
        {
            var address = new Address(cid, oid);
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.GetObjectHeader(address, false, false, new CallOptions { Ttl = 2 }, source.Token).Result;
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
            var o = client.GetObjectPayloadRangeData(address, new Object.Range { Offset = 0, Length = 3 }, false, new CallOptions { Ttl = 1 }, source.Token).Result;
            Console.WriteLine(o.ToHexString());
            Assert.AreEqual("hel", Encoding.ASCII.GetString(o));
        }

        [TestMethod]
        public void TestObjectGetRangeHash()
        {
            var address = new Address(cid, oid);
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var o = client.GetObjectPayloadRangeHash(address, new List<Object.Range> { new Object.Range { Offset = 0, Length = 3 }, new Object.Range { Offset = 1000, Length = 124 } }, ChecksumType.Tz, new byte[] { 0x00 }, new CallOptions { Ttl = 2 }, source.Token).Result;
            Console.WriteLine($"{o.Count} {string.Join(", ", o.Select(p => p.ToHexString()))}");
            Assert.AreEqual(1, o.Count);
        }

        [TestMethod]
        public void TestObjectSearch()
        {
            using var client = new Client.Client(key, host);
            using var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var filter = new SearchFilters();
            filter.AddTypeFilter(MatchType.StringEqual, ObjectType.Regular);
            var o = client.SearchObject(cid, filter, new CallOptions { Ttl = 2 }, source.Token).Result;
            Console.WriteLine($"{o.Count} {string.Join(", ", o.Select(p => p.String()))}");
            Assert.IsTrue(o.Contains(oid));
        }

        [TestMethod]
        public void TestClient()
        {
            var key = "7310c4da083264666cc3567d5cb4a5b060ca34fb68959de58bd04959f3cbc6b2".HexToBytes().LoadPrivateKey();
            _ = new Client.Client(key, "127.0.0.1:8080");
        }
    }
}
