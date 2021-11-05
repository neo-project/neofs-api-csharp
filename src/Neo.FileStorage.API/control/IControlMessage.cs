using Google.Protobuf;

namespace Neo.FileStorage.API.Control
{
    public interface IControlMessage
    {
        IMessage SignData { get; }
        Signature Signature { get; set; }
    }
}
