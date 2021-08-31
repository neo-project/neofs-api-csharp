using System;
using System.Threading.Tasks;
using Neo.FileStorage.API.Session;

namespace Neo.FileStorage.API.Client
{
    public interface IClientStream : IDisposable
    {
        Task Write(IRequest request);

        Task<IResponse> Close();
    }
}
