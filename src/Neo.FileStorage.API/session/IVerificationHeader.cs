using Google.Protobuf;
using Neo.FileStorage.API.Refs;

namespace Neo.FileStorage.API.Session
{
    public interface IVerificationHeader : IMessage
    {
        Signature BodySignature { get; set; }
        Signature MetaSignature { get; set; }
        Signature OriginSignature { get; set; }
        IVerificationHeader GetOrigin();
        void SetOrigin(IVerificationHeader verificationHeader);
    }
}
