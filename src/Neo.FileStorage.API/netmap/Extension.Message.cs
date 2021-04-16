using Google.Protobuf;
using Neo.FileStorage.API.Session;

namespace Neo.FileStorage.API.Netmap
{
    public partial class LocalNodeInfoRequest : IRequest
    {
        public IMessage GetBody()
        {
            return Body;
        }
    }

    public partial class LocalNodeInfoResponse : IResponse
    {
        public IMessage GetBody()
        {
            return Body;
        }
    }

    public partial class NetworkInfoRequest : IRequest
    {
        public IMessage GetBody()
        {
            return Body;
        }
    }

    public partial class NetworkInfoResponse : IResponse
    {
        public IMessage GetBody()
        {
            return Body;
        }
    }
}
