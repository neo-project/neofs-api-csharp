using Google.Protobuf;
using Neo.FileStorage.API.Refs;

namespace Neo.FileStorage.API.Session
{
    public interface IVerificationHeader : IMessage
    {
        Signature BodySignature { get; }
        Signature MetaSignature { get; }
        Signature OriginSignature { get; }
        IVerificationHeader GetOrigin();
    }
}
