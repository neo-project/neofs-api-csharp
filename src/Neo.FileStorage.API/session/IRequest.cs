using Neo.FileStorage.API.Session;
using Google.Protobuf;

namespace Neo.FileStorage.API.Session
{
    public interface IRequestMeta
    {
        RequestMetaHeader MetaHeader { get; set; }
    }

    public interface IRequest : IRequestMeta, IMessage
    {
        RequestVerificationHeader VerifyHeader { get; set; }
        IMessage GetBody();
    }
}