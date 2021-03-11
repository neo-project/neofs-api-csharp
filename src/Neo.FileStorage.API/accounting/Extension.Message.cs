using Google.Protobuf;
using Neo.FileStorage.API.Session;

namespace Neo.FileStorage.API.Accounting
{
    public partial class BalanceRequest : IRequest
    {
        public IMessage GetBody()
        {
            return Body;
        }
    }

    public partial class BalanceResponse : IResponse
    {
        public IMessage GetBody()
        {
            return Body;
        }
    }
}
