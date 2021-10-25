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
        // private readonly string host = "http://s04.neofs.devenv:8080";
        private readonly string host = "http://localhost:8080";
        // private readonly ContainerID cid = ContainerID.FromString("8tqfNtRyBFhzRZnFkddVBxsYEQWjJes37Lfdq3p9R7Ge");//not in policy
        // private readonly ObjectID oid = ObjectID.FromString("CkQsya9r9cAgiHqk5GLXY34zdTWUKwQ7KmVSsMXfrKVe");//small
        // private readonly ObjectID oid = ObjectID.FromString("");//big
        // private readonly ObjectID oid = ObjectID.FromString("HVXgYePb1FKSidNWwdHD6tHNnFN6kfN2J6GUvc8Tg8ym");//split
        // private readonly ContainerID cid = ContainerID.FromString("HDDXsEuu9kEkewxuvh2zpZtvYU9uLCDXV99zwJ31romy");//the only one in policy
        // private readonly ObjectID oid = ObjectID.FromString("CGwDnGaCYEFVM97WtvhBZ2NEaErMV2bSERETJ468M3bW");//small
        // private readonly ObjectID oid = ObjectID.FromString("");//big
        // private readonly ObjectID oid = ObjectID.FromString("JDYccUD7eSk3MNhdGWRK3Hw3vgfSjnBj3KuvopE87m6P");//split
        private readonly ContainerID cid = ContainerID.FromString("D8JeYhQgr7SpZAL2fsGYzG9P4yMctSDe21B9S8q1Q92A");//with others in policy
        private readonly ObjectID oid = ObjectID.FromString("9rQuZbCxiv1XN8NZ2EafpPWifvPBbzobTgHyMZJQoEUf");//small
        // private readonly ContainerID cid = ContainerID.FromString("41MC5Q7kZ5NBvQV1qGV2kVVdEkymJwzYtgWZ5yzynqCv");// only one go node
        // private readonly ObjectID oid = ObjectID.FromString("2sTv7ZwchLHg5XDqscnowGNBfHTELCEmYBW6CFXREpuy"); //small
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
