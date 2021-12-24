namespace Neo.FileStorage.API.Session
{
    public partial class RequestVerificationHeader : IVerificationHeader
    {
        public IVerificationHeader GetOrigin()
        {
            return Origin;
        }
    }

    public partial class ResponseVerificationHeader : IVerificationHeader
    {
        public IVerificationHeader GetOrigin()
        {
            return Origin;
        }
    }
}
