using Neo.FileStorage.API.Refs;

namespace Neo.FileStorage.API.Session
{
    public partial class RequestMetaHeader : IMetaHeader
    {
        public static RequestMetaHeader Default
        {
            get
            {
                var meta = new RequestMetaHeader()
                {
                    Version = Version.SDKVersion(),
                    Epoch = 0,
                    Ttl = 2,
                };
                return meta;
            }
        }

        public IMetaHeader GetOrigin()
        {
            return Origin;
        }
    }

    public partial class ResponseMetaHeader : IMetaHeader
    {
        public static ResponseMetaHeader Default
        {
            get
            {
                var meta = new ResponseMetaHeader()
                {
                    Version = new Version
                    {
                        Major = 2,
                        Minor = 0,
                    },
                    Epoch = 0,
                    Ttl = 1,
                };
                return meta;
            }
        }

        public IMetaHeader GetOrigin()
        {
            return Origin;
        }
    }
}
