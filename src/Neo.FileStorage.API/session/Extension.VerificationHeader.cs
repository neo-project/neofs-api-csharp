namespace Neo.FileStorage.API.Session
{
    public partial class RequestVerificationHeader : IVerificationHeader
    {
        IVerificationHeader IVerificationHeader.GetOrigin()
        {
            return Origin;
        }

        void IVerificationHeader.SetOrigin(IVerificationHeader verificationHeader)
        {
            Origin = (RequestVerificationHeader)verificationHeader;
        }
    }

    public partial class ResponseVerificationHeader : IVerificationHeader
    {
        IVerificationHeader IVerificationHeader.GetOrigin()
        {
            return Origin;
        }

        void IVerificationHeader.SetOrigin(IVerificationHeader verificationHeader)
        {
            Origin = (ResponseVerificationHeader)verificationHeader;
        }
    }
}
