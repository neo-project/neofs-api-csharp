using System;
using System.Threading.Tasks;
using Grpc.Core;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Object;

namespace Neo.FileStorage.API.Client
{
    public class PutStream : IDisposable
    {
        public AsyncClientStreamingCall<PutRequest, PutResponse> Call { get; init; }

        public void Dispose()
        {
            Call.Dispose();
        }

        public async void Write(PutRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (request.Body?.ObjectPartCase != PutRequest.Types.Body.ObjectPartOneofCase.Chunk) throw new ArgumentException("invalid requst type, expect chunk");
            await Call.RequestStream.WriteAsync(request);
        }

        public async Task<PutResponse> Close()
        {
            await Call.RequestStream.CompleteAsync();
            var resp = await Call.ResponseAsync;
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid object put response");
            return resp;
        }
    }
}
