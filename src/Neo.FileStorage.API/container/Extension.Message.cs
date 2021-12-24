using Google.Protobuf;
using Neo.FileStorage.API.Session;

namespace Neo.FileStorage.API.Container
{
    public partial class AnnounceUsedSpaceRequest : IRequest
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

    public partial class AnnounceUsedSpaceResponse : IResponse
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

    public partial class GetRequest : IRequest
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

    public partial class GetResponse : IResponse
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

    public partial class PutRequest : IRequest
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

    public partial class PutResponse : IResponse
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

    public partial class DeleteRequest : IRequest
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

    public partial class DeleteResponse : IResponse
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

    public partial class ListRequest : IRequest
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

    public partial class ListResponse : IResponse
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

    public partial class SetExtendedACLRequest : IRequest
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

    public partial class SetExtendedACLResponse : IResponse
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

    public partial class GetExtendedACLRequest : IRequest
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

    public partial class GetExtendedACLResponse : IResponse
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
