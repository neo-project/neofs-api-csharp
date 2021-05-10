using Google.Protobuf;
using Neo.FileStorage.API.Session;

namespace Neo.FileStorage.API.Reputation
{
    public sealed partial class AnnounceLocalTrustRequest : IRequest
    {
        public IMessage GetBody()
        {
            return Body;
        }
    }

    public sealed partial class AnnounceLocalTrustResponse : IResponse
    {
        public IMessage GetBody()
        {
            return Body;
        }
    }

    public sealed partial class AnnounceIntermediateResultRequest : IRequest
    {
        public IMessage GetBody()
        {
            return Body;
        }
    }

    public sealed partial class AnnounceIntermediateResultResponse : IResponse
    {
        public IMessage GetBody()
        {
            return Body;
        }
    }
}
