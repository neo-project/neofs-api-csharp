// Copyright (C) 2015-2025 The Neo Project.
//
// ObjectReader.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Grpc.Core;
using Neo.FileStorage.API.Object;
using System;

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
