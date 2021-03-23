using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Acl;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Netmap;
using Neo.FileStorage.API.Refs;
using System;
using System.Threading;

namespace Neo.FileStorage.API.UnitTests.FSClient
{
    [TestClass]
    public class UT_Container
    {
        [TestMethod]
        public void TestPutContainer()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var client = new Client.Client(key, host);
            var replica = new Replica(1, "");
            var policy = new PlacementPolicy(2, new Replica[] { replica }, null, null);
            var container = new Container.Container
            {
                Version = Refs.Version.SDKVersion(),
                OwnerId = key.ToOwnerID(),
                Nonce = Guid.NewGuid().ToByteString(),
                BasicAcl = (uint)BasicAcl.PublicBasicRule,
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
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var client = new Client.Client(key, host);
            var cid = ContainerID.FromBase58String("8F2ZAdt6XDkBXwFcV3rQAMu42cr2zsxWy6WLmTjiErew");
            var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            var container = client.GetContainer(cid, context: source.Token).Result;
            Assert.AreEqual(cid, container.CalCulateAndGetId);
        }

        [TestMethod]
        public void TestDeleteContainer()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var client = new Client.Client(key, host);
            var cid = ContainerID.FromBase58String("Bun3sfMBpnjKc3Tx7SdwrMxyNi8ha8JT3dhuFGvYBRTz");
            var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            client.DeleteContainer(cid, context: source.Token).Wait();
        }

        [TestMethod]
        public void TestListContainer()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var client = new Client.Client(key, host);
            var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            var cids = client.ListContainers(key.ToOwnerID(), context: source.Token).Result;
            Assert.AreEqual(1, cids.Count);
            Assert.AreEqual("Bun3sfMBpnjKc3Tx7SdwrMxyNi8ha8JT3dhuFGvYBRTz", cids[0].ToBase58String());
        }

        [TestMethod]
        public void TestGetExtendedACL()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var cid = ContainerID.FromBase58String("Bun3sfMBpnjKc3Tx7SdwrMxyNi8ha8JT3dhuFGvYBRTz");
            var client = new Client.Client(key, host);
            var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            var eacl = client.GetEACL(cid, context: source.Token);
            Console.WriteLine(eacl);
        }

        [TestMethod]
        public void TestSetExtendedACL()
        {
            var host = "localhost:8080";
            var key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
            var cid = ContainerID.FromBase58String("Bun3sfMBpnjKc3Tx7SdwrMxyNi8ha8JT3dhuFGvYBRTz");
            var client = new Client.Client(key, host);
            var eacl = new EACLTable
            {
                Version = Refs.Version.SDKVersion(),
                ContainerId = cid,
            };
            var filter = new EACLRecord.Types.Filter
            {
                HeaderType = HeaderType.HeaderUnspecified,
                MatchType = MatchType.StringEqual,
                Key = "test",
                Value = "test"
            };
            var target = new EACLRecord.Types.Target
            {
                Role = Role.Others,
            };
            var record = new EACLRecord
            {
                Operation = Acl.Operation.Get,
                Action = Acl.Action.Deny,
            };
            record.Filters.Add(filter);
            record.Targets.Add(target);
            var source = new CancellationTokenSource();
            source.CancelAfter(10000);
            client.SetEACL(eacl, context: source.Token).Wait();
        }
    }
}
