using System.Security.Cryptography;
using Neo.FileStorage.API.Cryptography;

namespace Neo.FileStorage.API.Session
{
    public sealed partial class SessionToken
    {
        public static partial class Types
        {
            public sealed partial class Body
            {
                public static partial class Types
                {
                    public sealed partial class TokenLifetime
                    {
                    }
                }
            }
        }

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
