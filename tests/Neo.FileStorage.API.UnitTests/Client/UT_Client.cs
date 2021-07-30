using System;
using System.Security.Cryptography;
using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.Cryptography;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Cryptography.Tz;
using Neo.FileStorage.API.Object;
using Neo.FileStorage.API.Refs;

namespace Neo.FileStorage.API.UnitTests.FSClient
{
    [TestClass]
    public partial class UT_Client
    {
        private readonly string host = "http://st1.storage.fs.neo.org:8080";
        private readonly ContainerID cid = ContainerID.FromString("3NYxMpbnNoRYtrvP1Z9AbbSoS7gSAx2R7bcvDmZ7bz1r");
        private readonly ObjectID oid = ObjectID.FromString("2FNDyiLSabWCmoyWRw1YgWB8NcJVf4UncQ3QrFKSBrYp");
        private readonly ECDsa key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();

        public Object.Object RandomFullObject()
        {
            var rand = new Random();
            var payload = new byte[1024];
            rand.NextBytes(payload);
            var obj = new Object.Object
            {
                Header = new Header
                {
                    Version = Refs.Version.SDKVersion(),
                    OwnerId = OwnerID.FromScriptHash(key.PublicKey().PublicKeyToScriptHash()),
                    ContainerId = cid,
                    ObjectType = ObjectType.Regular,
                    PayloadHash = new Checksum
                    {
                        Type = ChecksumType.Sha256,
                        Sum = ByteString.CopyFrom(payload.Sha256()),
                    },
                    HomomorphicHash = new Checksum
                    {
                        Type = ChecksumType.Tz,
                        Sum = ByteString.CopyFrom(new TzHash().ComputeHash(payload)),
                    },
                    PayloadLength = (ulong)payload.Length,
                },
                Payload = ByteString.CopyFrom(payload),
            };
            obj.ObjectId = obj.CalculateID();
            obj.Signature = obj.CalculateIDSignature(key);
            return obj;
        }
    }
}
