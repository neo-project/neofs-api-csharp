using Neo.FileStorage.API.Session;
using Google.Protobuf;

namespace Neo.FileStorage.API.Session
{
    public interface IResponseMeta
    {
        ResponseMetaHeader MetaHeader { get; set; }
    }

    public interface IResponse : IResponseMeta
    {
        ResponseVerificationHeader VerifyHeader { get; set; }
        IMessage GetBody();
    }
}