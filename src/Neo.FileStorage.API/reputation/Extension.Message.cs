using Google.Protobuf;
using Neo.FileStorage.API.Session;

namespace Neo.FileStorage.API.Reputation
{
    public sealed partial class AnnounceLocalTrustRequest : IRequest
    {
        IMetaHeader IVerificableMessage.GetMetaHeader()
        {
            return MetaHeader;
        }

        IVerificationHeader IVerificableMessage.GetVerificationHeader()
        {
            return VerifyHeader;
        }

        public IMessage GetBody()
        {
            return Body;
        }
    }

    public sealed partial class AnnounceLocalTrustResponse : IResponse
    {
        IMetaHeader IVerificableMessage.GetMetaHeader()
        {
            return MetaHeader;
        }

        IVerificationHeader IVerificableMessage.GetVerificationHeader()
        {
            return VerifyHeader;
        }

        public IMessage GetBody()
        {
            return Body;
        }
    }

    public sealed partial class AnnounceIntermediateResultRequest : IRequest
    {
        IMetaHeader IVerificableMessage.GetMetaHeader()
        {
            return MetaHeader;
        }

        IVerificationHeader IVerificableMessage.GetVerificationHeader()
        {
            return VerifyHeader;
        }

        public IMessage GetBody()
        {
            return Body;
        }
    }

    public sealed partial class AnnounceIntermediateResultResponse : IResponse
    {
        IMetaHeader IVerificableMessage.GetMetaHeader()
        {
            return MetaHeader;
        }

        IVerificationHeader IVerificableMessage.GetVerificationHeader()
        {
            return VerifyHeader;
        }

        public IMessage GetBody()
        {
            return Body;
        }
    }
}
