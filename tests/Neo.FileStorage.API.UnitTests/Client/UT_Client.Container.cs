using System;
using System.Security.Cryptography;
using System.Threading;
using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Acl;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Netmap;
using Neo.FileStorage.API.Refs;

namespace Neo.FileStorage.API.UnitTests.FSClient
{
    public partial class UT_Client
    {
        [TestMethod]
        public void TestPutContainer()
        {
            using var client = new Client.Client(key, host);
            var replica = new Replica(1, "");
            var policy = new PlacementPolicy(1, new Replica[] { replica }, null, null);
            var container = new Container.Container
            {
                Version = Refs.Version.SDKVersion(),
                OwnerId = OwnerID.FromScriptHash(key.PublicKey().PublicKeyToScriptHash()),
                Nonce = Guid.NewGuid().ToByteString(),
                BasicAcl = 536862719u,
                PlacementPolicy = policy,
            };
            container.Attributes.Add(new Container.Container.Types.Attribute
            {
                Key = "CreatedAt",
                Value = DateTime.UtcNow.ToString(),
            });
            var source = new CancellationTokenSource();
            source.CancelAfter(TimeSpan.FromMinutes(1));
            var cid = client.PutContainer(container, context: source.Token).Result;
            Console.WriteLine(cid.ToBase58String());
            Assert.AreEqual(container.CalCulateAndGetId, cid);
        }

        [TestMethod]
        public void TestGetContainer()
        {
            using var client = new Client.Client(key, host);
            var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            var container = client.GetContainer(cid, context: source.Token).Result;
            Assert.AreEqual(cid, container.Container.CalCulateAndGetId);
            Console.WriteLine(container.Container);
        }

        [TestMethod]
        public void TestDeleteContainer()
        {
            using var client = new Client.Client(key, host);
            var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            client.DeleteContainer(cid, context: source.Token).Wait();
        }

        [TestMethod]
        public void TestListContainer()
        {
            using var client = new Client.Client(key, host);
            var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            var cids = client.ListContainers(OwnerID.FromScriptHash(key.PublicKey().PublicKeyToScriptHash()), context: source.Token).Result;
            Assert.AreEqual(1, cids.Count);
            Assert.AreEqual("Bun3sfMBpnjKc3Tx7SdwrMxyNi8ha8JT3dhuFGvYBRTz", cids[0].ToBase58String());
        }

        [TestMethod]
        public void TestGetExtendedACL()
        {
            using var client = new Client.Client(key, host);
            var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            var eacl = client.GetEAcl(cid, context: source.Token).Result;
            Console.WriteLine(eacl.Table.ToString());
        }

        [TestMethod]
        public void TestSetExtendedACL()
        {
            using var client = new Client.Client(key, host);
            var target = new EACLRecord.Types.Target
            {
                Role = Role.Others,
            };
            var record = new EACLRecord
            {
                Operation = API.Acl.Operation.Delete,
                Action = API.Acl.Action.Deny,
            };
            record.Targets.Add(target);
            var eacl = new EACLTable
            {
                Version = Refs.Version.SDKVersion(),
                ContainerId = cid,
            };
            eacl.Records.Add(record);
            var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            client.SetEACL(eacl, context: source.Token).Wait();
        }
    }
}
