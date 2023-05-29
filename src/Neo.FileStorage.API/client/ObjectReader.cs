using System;
using Grpc.Core;
using Neo.FileStorage.API.Object;

namespace Neo.FileStorage.API.Client
{
    public sealed class ObjectReader : IDisposable
    {
        public AsyncServerStreamingCall<GetResponse> Call { get; init; }

        public Object.Object ReadHeader()
        {
            if (!Call.ResponseStream.MoveNext().Result)
                throw new InvalidOperationException("unexpect end of stream");
            var response = Call.ResponseStream.Current;
            if (response.Body.ObjectPartCase != GetResponse.Types.Body.ObjectPartOneofCase.Init)
                throw new InvalidOperationException("unexpect message type");
            return new Object.Object
            {
                ObjectId = response.Body.Init.ObjectId,
                Signature = response.Body.Init.Signature,
                Header = response.Body.Init.Header,
            };
        }

        public (byte[], bool) ReadChunk()
        {
            if (!Call.ResponseStream.MoveNext().Result)
                return (null, false);
            var response = Call.ResponseStream.Current;
            if (response.Body.ObjectPartCase != GetResponse.Types.Body.ObjectPartOneofCase.Chunk)
                throw new InvalidOperationException("unexpect message type");
            return (response.Body.Chunk.ToByteArray(), true);
        }

        public void Dispose()
        {
            Call?.Dispose();
        }
    }
}
