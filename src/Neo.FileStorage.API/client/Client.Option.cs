using Neo.FileStorage.API.Session;
using System;

namespace Neo.FileStorage.API.Client
{
    public sealed partial class Client
    {
        private Action<IResponse> responseHandler;

        public Client WithResponseInfoHandler(Action<IResponse> handler)
        {
            responseHandler = handler;
            return this;
        }
    }
}
