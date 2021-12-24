using Google.Protobuf;

namespace Neo.FileStorage.API.Session
{
    public partial class CreateResponse : IResponse
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

    public partial class CreateRequest : IRequest
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
