using Neo.FileStorage.API.Refs;
using Neo.FileStorage.API.Session;

namespace Neo.FileStorage.API.Client
{
    public class ContainerWithSignature
    {
        public Container.Container Container;
        public SignatureRFC6979 Signature;
        public SessionToken SessionToken;
    }
}
