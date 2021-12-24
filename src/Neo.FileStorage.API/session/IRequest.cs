namespace Neo.FileStorage.API.Session
{
    public interface IRequest : IVerificableMessage
    {
        RequestMetaHeader MetaHeader { get; set; }
        RequestVerificationHeader VerifyHeader { get; set; }
    }
}
