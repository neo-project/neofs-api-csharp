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
        // private readonly string host = "https://st1.storage.fs.neo.org:8082";
        // private readonly string host = "https://st01.testnet.fs.neo.org";
        // private readonly string host = "http://s03.neofs.devenv:8080";
        private readonly string host = "http://localhost:8080";
        // private readonly ContainerID cid = ContainerID.FromString("HdzvfP2nMvAotg1xkpKRXThZwRsGg8ubXuiMVJH59nR6");//not in policy
        // private readonly ObjectID oid = ObjectID.FromString("FHbQzPJn39g26d3rAuTK1Nru6f7m3ko4WwVHF8veZBsc");//small
        // private readonly ObjectID oid = ObjectID.FromString("9nYH38W9aCKPLU3kc7sVfCku1Ya2YBVKnoxbJYwpEPB4");//big
        // private readonly ObjectID oid = ObjectID.FromString("Cngwmhh4eviAjb5saSgJqKTxsQLHUQAu1DL2cS9khz8E");//split
        private readonly ContainerID cid = ContainerID.FromString("Fyhrr5pGEKUvJDJYKqomMeXfsPqwBSXeqwFiVvLnZhRD");//the only one in policy
        // private readonly ObjectID oid = ObjectID.FromString("J8d6DMVr3FYjNXeXywSTtfYH65e85Tr71bYZmN5ee8gb");//small
        // private readonly ObjectID oid = ObjectID.FromString("7p8HjxgXZzSUePZ7CyTN5nFrLRG1FhoYFoLjpUnnYW69");//big
        private readonly ObjectID oid = ObjectID.FromString("6kECKQXucQyZKmAeTEKzZToYjgX4DmYbjV2siZrRoDUi");//split
        private readonly ECDsa key = "Kzj1LbTtmfbyJjn4cZhD6U4pdq74iHcmKmGRRBiLQoQzPBRWLEKz".LoadWif();

        public Object.Object RandomFullObject(int len = 1024)
        {
            var rand = new Random();
            var payload = new byte[len];
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
            obj.Header.Attributes.Add(new Header.Types.Attribute
            {
                Key = "category",
                Value = "test"
            });
            obj.ObjectId = obj.CalculateID();
            obj.Signature = obj.CalculateIDSignature(key);
            return obj;
        }
    }
}
