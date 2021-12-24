using Google.Protobuf;

namespace Neo.FileStorage.API.Session
{
    public interface IVerificableMessage
    {
        IMetaHeader GetMetaHeader();
        IVerificationHeader GetVerificationHeader();
        IMessage GetBody();
    }
}
