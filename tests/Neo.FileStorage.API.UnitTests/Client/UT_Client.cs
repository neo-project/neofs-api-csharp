using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Refs;

namespace Neo.FileStorage.API.UnitTests.FSClient
{
    [TestClass]
    public partial class UT_Client
    {
        private readonly string host = "http://localhost:8080";
        private readonly ContainerID cid = ContainerID.FromBase58String("FeDH8Gnri5KJjkPSofjcMeX37KUScYaxAKFEzoNorsJG");
        private readonly ObjectID oid = ObjectID.FromBase58String("7Q7fPKESmRJ1VGHKcB2pB4JWVebsQzrJypwQiNLU1ekv");
        private readonly ECDsa key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
    }
}
