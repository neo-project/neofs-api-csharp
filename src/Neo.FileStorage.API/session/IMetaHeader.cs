using Google.Protobuf;

namespace Neo.FileStorage.API.Session
{
    public interface IMetaHeader : IMessage
    {
        IMetaHeader GetOrigin();
    }
}
