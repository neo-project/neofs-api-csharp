
using System.Security.Cryptography;
using Neo.FileStorage.API.Cryptography;

namespace Neo.FileStorage.API.Reputation
{
    public partial class GlobalTrust
    {
        public void Sign(ECDsa key)
        {
            Signature = key.SignMessagePart(Body);
        }

        public bool VerifySignature()
        {
            return Signature.VerifyMessagePart(Body);
        }
    }
}
