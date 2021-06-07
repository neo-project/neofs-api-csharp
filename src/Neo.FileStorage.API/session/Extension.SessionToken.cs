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
                    json["ownerid"] = OwnerId?.ToJson();
                    json["lifetime"] = Lifetime?.ToJson();
                    json["sessionkey"] = SessionKey.ToBase64();
                    json["object"] = Object?.ToJson();
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
            json["sinature"] = Signature?.ToJson();
            return json;
        }
    }
}