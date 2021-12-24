using Google.Protobuf;
using Neo.FileStorage.API.Refs;

namespace Neo.FileStorage.API.Session
{
    public interface IMetaHeader : IMessage
    {
        IMetaHeader GetOrigin();
    }
}
