using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Refs;

namespace Neo.FileStorage.API.UnitTests.FSClient
{
    [TestClass]
    public partial class UT_Client
    {
        private readonly string host = "http://st1.storage.fs.neo.org:8080";
        private readonly ContainerID cid = ContainerID.FromBase58String("3NYxMpbnNoRYtrvP1Z9AbbSoS7gSAx2R7bcvDmZ7bz1r");
        private readonly ObjectID oid = ObjectID.FromBase58String("2FNDyiLSabWCmoyWRw1YgWB8NcJVf4UncQ3QrFKSBrYp");
        private readonly ECDsa key = "KxDgvEKzgSBPPfuVfw67oPQBSjidEiqTHURKSDL1R7yGaGYAeYnr".LoadWif();
    }
}
