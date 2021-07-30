using System.Security.Cryptography;
using Neo.FileStorage.API.Cryptography;
using Neo.IO.Json;

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
                        public JObject ToJson()
                        {
                            var json = new JObject();
                            json["exp"] = Exp;
                            json["nbf"] = Nbf;
                            json["iat"] = Iat;
                            return json;
                        }
                    }
                }

                public JObject ToJson()
                {
                    var json = new JObject();
                    json["id"] = Id?.ToBase64();
                    json["ownerID"] = OwnerId?.ToJson();
                    json["lifetime"] = Lifetime?.ToJson();
                    json["sessionKey"] = SessionKey.ToBase64();
                    if (ContextCase == ContextOneofCase.Object)
                        json["object"] = Object.ToJson();
                    else if (ContextCase == ContextOneofCase.Container)
                        json["container"] = Container.ToJson();
                    return json;
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

        public JObject ToJson()
        {
            var json = new JObject();
            json["body"] = Body?.ToJson();
            json["signature"] = Signature?.ToJson();
            return json;
        }
    }
}