using Google.Protobuf;

namespace Neo.FileStorage.API.Session
{
    public interface IVerificableMessage : IMessage
    {
        IMetaHeader GetMetaHeader();
        void SetMetaHeader(IMetaHeader metaHeader);
        IVerificationHeader GetVerificationHeader();
        void SetVerificationHeader(IVerificationHeader verificationHeader);
        IMessage GetBody();
    }
}
