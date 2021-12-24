namespace Neo.FileStorage.API.Session
{
    public interface IResponse : IVerificableMessage
    {
        ResponseMetaHeader MetaHeader { get; set; }
        ResponseVerificationHeader VerifyHeader { get; set; }
    }
}
