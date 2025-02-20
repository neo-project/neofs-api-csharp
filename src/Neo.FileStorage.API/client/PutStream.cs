// Copyright (C) 2015-2025 The Neo Project.
//
// PutStream.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Grpc.Core;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Object;
using Neo.FileStorage.API.Session;
using System;
using System.Threading.Tasks;

namespace Neo.FileStorage.API.Client
{
    public sealed class PutStream : IClientStream, IDisposable
    {
        public AsyncClientStreamingCall<PutRequest, PutResponse> Call { get; init; }

        public void Dispose()
        {
            Call?.Dispose();
        }

        public async Task Write(IRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));
            if (request is PutRequest putRequest)
            {
                if (putRequest.Body?.ObjectPartCase != PutRequest.Types.Body.ObjectPartOneofCase.Chunk) throw new ArgumentException("invalid requst type, expect chunk");
                await Call.RequestStream.WriteAsync(putRequest);
            }
            else
                throw new InvalidOperationException("invalid request type");
        }

        private void CheckStatus(IResponse resp)
        {
            var meta = resp.MetaHeader;
            if (meta?.Status is not null && !meta.Status.IsSuccess())
            {
                throw new RpcException(meta.Status.ToGrpcStatus());
            }
        }

        public async Task<IResponse> Close()
        {
            await Call.RequestStream.CompleteAsync();
            var resp = await Call.ResponseAsync;
            if (!resp.Verify())
                throw new InvalidOperationException("invalid object put response");
            CheckStatus(resp);
            return resp;
        }
    }
}
