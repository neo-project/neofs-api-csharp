using Neo.FileStorage.API.Acl;
using Neo.FileStorage.API.Refs;
using Neo.FileStorage.API.Session;

namespace Neo.FileStorage.API.Client
{
    public class EAclWithSignature
    {
        public EACLTable Table;
        public SignatureRFC6979 Signature;
        public SessionToken SessionToken;
    }
}
