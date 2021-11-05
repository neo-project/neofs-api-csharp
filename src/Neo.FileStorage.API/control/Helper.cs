using Google.Protobuf;
using Neo.FileStorage.API.Cryptography;
using System.Security.Cryptography;

namespace Neo.FileStorage.API.Control
{
    public static class Helper
    {
        public static bool VerifyControlMessage(this IControlMessage message)
        {
            using var key = message.Signature.Key.ToByteArray().LoadPublicKey();
            return key.VerifyData(message.SignData.ToByteArray(), message.Signature.Sign.ToByteArray());
        }

        public static void SignControlMessage(this ECDsa key, IControlMessage message)
        {
            message.Signature = new()
            {
                Key = ByteString.CopyFrom(key.PublicKey()),
                Sign = ByteString.CopyFrom(key.SignData(message.SignData.ToByteArray())),
            };
        }
    }
}
