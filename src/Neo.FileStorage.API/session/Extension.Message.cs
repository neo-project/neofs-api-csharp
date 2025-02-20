// Copyright (C) 2015-2025 The Neo Project.
//
// Extension.Message.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

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

        void IVerificableMessage.SetMetaHeader(IMetaHeader metaHeader)
        {
            MetaHeader = (ResponseMetaHeader)metaHeader;
        }

        void IVerificableMessage.SetVerificationHeader(IVerificationHeader verificationHeader)
        {
            VerifyHeader = (ResponseVerificationHeader)verificationHeader;
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

        void IVerificableMessage.SetMetaHeader(IMetaHeader metaHeader)
        {
            MetaHeader = (RequestMetaHeader)metaHeader;
        }

        void IVerificableMessage.SetVerificationHeader(IVerificationHeader verificationHeader)
        {
            VerifyHeader = (RequestVerificationHeader)verificationHeader;
        }

        public IMessage GetBody()
        {
            return Body;
        }
    }
}
