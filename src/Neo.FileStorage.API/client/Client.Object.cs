// Copyright (C) 2015-2025 The Neo Project.
//
// Client.Object.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Google.Protobuf;
using Grpc.Core;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Object;
using Neo.FileStorage.API.Refs;
using Neo.FileStorage.API.Session;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Neo.FileStorage.API.Client
{
    public sealed partial class Client
    {
        public async Task<Object.Object> GetObject(Address address, bool raw = false, CallOptions options = null, CancellationToken context = default)
        {
            if (address is null) throw new ArgumentNullException(nameof(address));
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var req = new GetRequest
            {
                MetaHeader = opts.GetRequestMetaHeader(),
                Body = new GetRequest.Types.Body
                {
                    Raw = raw,
                    Address = address,
                }
            };
            PrepareObjectSessionToken(req.MetaHeader, opts.Key, address, ObjectSessionContext.Types.Verb.Get);
            opts.Key.Sign(req);

            return await GetObject(req, opts.Deadline, context);
        }

        public ObjectReader GetObjectInit(Address address, bool raw = false, CallOptions options = null, CancellationToken context = default)
        {
            if (address is null) throw new ArgumentNullException(nameof(address));
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var req = new GetRequest
            {
                MetaHeader = opts.GetRequestMetaHeader(),
                Body = new GetRequest.Types.Body
                {
                    Raw = raw,
                    Address = address,
                }
            };
            PrepareObjectSessionToken(req.MetaHeader, opts.Key, address, ObjectSessionContext.Types.Verb.Get);
            opts.Key.Sign(req);
            return new ObjectReader
            {
                Call = ObjectClient.Get(req, cancellationToken: context)
            };
        }

        public async Task<Object.Object> GetObject(Address address, Stream payloadWriter, bool raw = false, CallOptions options = null, CancellationToken context = default)
        {
            if (address is null) throw new ArgumentNullException(nameof(address));
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var req = new GetRequest
            {
                MetaHeader = opts.GetRequestMetaHeader(),
                Body = new GetRequest.Types.Body
                {
                    Raw = raw,
                    Address = address,
                }
            };
            PrepareObjectSessionToken(req.MetaHeader, opts.Key, address, ObjectSessionContext.Types.Verb.Get);
            opts.Key.Sign(req);
            using var call = ObjectClient.Get(req, cancellationToken: context);
            var obj = new Object.Object();
            int offset = 0;
            while (await call.ResponseStream.MoveNext())
            {
                var resp = call.ResponseStream.Current;
                ProcessResponse(resp);
                switch (resp.Body.ObjectPartCase)
                {
                    case GetResponse.Types.Body.ObjectPartOneofCase.Init:
                        {
                            obj.ObjectId = resp.Body.Init.ObjectId;
                            obj.Signature = resp.Body.Init.Signature;
                            obj.Header = resp.Body.Init.Header;
                            break;
                        }
                    case GetResponse.Types.Body.ObjectPartOneofCase.Chunk:
                        {
                            if (obj.Header is null) throw new InvalidOperationException("missing init");
                            var chunk = resp.Body.Chunk.ToByteArray();
                            if (obj.PayloadSize < (ulong)(offset + chunk.Length))
                                throw new InvalidOperationException("data exceeds PayloadSize");
                            payloadWriter.Write(chunk, 0, chunk.Length);
                            offset += chunk.Length;
                            break;
                        }
                    case GetResponse.Types.Body.ObjectPartOneofCase.SplitInfo:
                        {
                            throw new SplitInfoException(resp.Body.SplitInfo);
                        }
                    default:
                        throw new FormatException("malformed object get reponse");
                }
            }
            if ((ulong)offset < obj.PayloadSize)
                throw new InvalidOperationException("data is less than PayloadSize");
            return obj;
        }

        public async Task<Object.Object> GetObject(GetRequest request, DateTime? deadline = null, CancellationToken context = default)
        {
            using var call = ObjectClient.Get(request, deadline: deadline, cancellationToken: context);
            var obj = new Object.Object();
            var payload = Array.Empty<byte>();
            int offset = 0;
            while (await call.ResponseStream.MoveNext())
            {
                var resp = call.ResponseStream.Current;
                ProcessResponse(resp);
                switch (resp.Body.ObjectPartCase)
                {
                    case GetResponse.Types.Body.ObjectPartOneofCase.Init:
                        {
                            obj.ObjectId = resp.Body.Init.ObjectId;
                            obj.Signature = resp.Body.Init.Signature;
                            obj.Header = resp.Body.Init.Header;
                            payload = new byte[obj.PayloadSize];
                            break;
                        }
                    case GetResponse.Types.Body.ObjectPartOneofCase.Chunk:
                        {
                            if (payload.Length == 0) throw new InvalidOperationException("missing init");
                            var chunk = resp.Body.Chunk;
                            if (obj.PayloadSize < (ulong)(offset + chunk.Length))
                                throw new InvalidOperationException("data exceeds PayloadSize");
                            resp.Body.Chunk.CopyTo(payload, offset);
                            offset += chunk.Length;
                            break;
                        }
                    case GetResponse.Types.Body.ObjectPartOneofCase.SplitInfo:
                        {
                            throw new SplitInfoException(resp.Body.SplitInfo);
                        }
                    default:
                        throw new FormatException("malformed object get reponse");
                }
            }
            if ((ulong)offset < obj.PayloadSize)
                throw new InvalidOperationException("data is less than PayloadSize");
            obj.Payload = ByteString.CopyFrom(payload);
            return obj;
        }

        public async Task<ObjectID> PutObject(Header header, Stream reader, CallOptions options = null, CancellationToken context = default)
        {
            if (header is null) throw new ArgumentNullException(nameof(header));
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var address = new Address
            {
                ContainerId = header.ContainerId,
                ObjectId = new()
                {
                    Value = header.Sha256()
                }
            };
            var req = new PutRequest
            {
                MetaHeader = opts.GetRequestMetaHeader(),
                Body = new()
                {
                    Init = new PutRequest.Types.Body.Types.Init
                    {
                        Header = header,
                    },
                }
            };
            PrepareObjectSessionToken(req.MetaHeader, opts.Key, address, ObjectSessionContext.Types.Verb.Put);
            opts.Key.Sign(req);

            using var stream = await PutObject(req, context: context);
            var block = new byte[Object.Object.ChunkSize];
            var count = reader.Read(block, 0, Object.Object.ChunkSize);
            while (count > 0)
            {
                var chunk_body = new PutRequest.Types.Body
                {
                    Chunk = ByteString.CopyFrom(block[..count]),
                };
                req.Body = chunk_body;
                req.VerifyHeader = null;
                opts.Key.Sign(req);
                await stream.Write(req);
                count = reader.Read(block, 0, Object.Object.ChunkSize);
            }
            var resp = (PutResponse)await stream.Close();
            return resp.Body.ObjectId;
        }

        public async Task<ObjectID> PutObject(Object.Object obj, CallOptions options = null, CancellationToken context = default)
        {
            if (obj is null) throw new ArgumentNullException(nameof(obj));
            if (obj.Header is null) throw new ArgumentException($"No Header in {nameof(obj)}");
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var address = new Address
            {
                ContainerId = obj.Header.ContainerId,
                ObjectId = obj.ObjectId ?? obj.CalculateID(),
            };
            var req = new PutRequest()
            {
                MetaHeader = opts.GetRequestMetaHeader(),
                Body = new PutRequest.Types.Body
                {
                    Init = new PutRequest.Types.Body.Types.Init
                    {
                        ObjectId = obj.ObjectId,
                        Signature = obj.Signature,
                        Header = obj.Header,
                    },
                }
            };
            PrepareObjectSessionToken(req.MetaHeader, opts.Key, address, ObjectSessionContext.Types.Verb.Put);
            opts.Key.Sign(req);

            using var stream = await PutObject(req, context: context);

            int offset = 0;
            while (offset < obj.Payload.Length)
            {
                var end = offset + Object.Object.ChunkSize > obj.Payload.Length ? obj.Payload.Length : offset + Object.Object.ChunkSize;
                var chunk = ByteString.CopyFrom(obj.Payload.ToByteArray()[offset..end]);
                req.Body = new()
                {
                    Chunk = chunk,
                };
                req.VerifyHeader = null;
                opts.Key.Sign(req);
                await stream.Write(req);
                offset = end;
            }
            var resp = (PutResponse)await stream.Close();
            return resp.Body.ObjectId;
        }

        public async Task<IClientStream> PutObject(PutRequest init, DateTime? deadline = null, CancellationToken context = default)
        {
            if (init is null) throw new ArgumentNullException(nameof(init));
            if (init.Body?.ObjectPartCase != PutRequest.Types.Body.ObjectPartOneofCase.Init) throw new ArgumentException("invalid request type, expect init");
            var call = ObjectClient.Put(deadline: deadline, cancellationToken: context);
            await call.RequestStream.WriteAsync(init);
            return new PutStream { Call = call };
        }

        public async Task<Address> DeleteObject(Address address, CallOptions options = null, CancellationToken context = default)
        {
            if (address is null) throw new ArgumentNullException(nameof(address));
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var req = new DeleteRequest
            {
                MetaHeader = opts.GetRequestMetaHeader(),
                Body = new DeleteRequest.Types.Body
                {
                    Address = address,
                }
            };
            PrepareObjectSessionToken(req.MetaHeader, opts.Key, address, ObjectSessionContext.Types.Verb.Delete);
            opts.Key.Sign(req);

            return await DeleteObject(req, opts.Deadline, context);
        }

        public async Task<Address> DeleteObject(DeleteRequest request, DateTime? deadline = null, CancellationToken context = default)
        {
            var resp = await ObjectClient.DeleteAsync(request, deadline: deadline, cancellationToken: context);
            ProcessResponse(resp);
            return resp.Body.Tombstone;
        }

        public async Task<Object.Object> GetObjectHeader(Address address, bool minimal = false, bool raw = false, CallOptions options = null, CancellationToken context = default)
        {
            if (address is null) throw new ArgumentNullException(nameof(address));
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var req = new HeadRequest
            {
                MetaHeader = opts.GetRequestMetaHeader(),
                Body = new HeadRequest.Types.Body
                {
                    Address = address,
                    MainOnly = minimal,
                    Raw = raw,
                }
            };
            PrepareObjectSessionToken(req.MetaHeader, opts.Key, address, ObjectSessionContext.Types.Verb.Head);
            opts.Key.Sign(req);

            return await GetObjectHeader(req, opts.Deadline, context);
        }

        public async Task<Object.Object> GetObjectHeader(HeadRequest request, DateTime? deadline = null, CancellationToken context = default)
        {
            var resp = await ObjectClient.HeadAsync(request, deadline: deadline, cancellationToken: context);
            ProcessResponse(resp);
            var header = new Header();
            var sig = new Signature();
            switch (resp.Body.HeadCase)
            {
                case HeadResponse.Types.Body.HeadOneofCase.ShortHeader:
                    {
                        if (!request.Body.MainOnly) throw new FormatException("expect full header received short");
                        var short_header = resp.Body.ShortHeader;
                        if (short_header is null)
                            throw new FormatException("malformed object header");
                        header.PayloadLength = short_header.PayloadLength;
                        header.Version = short_header.Version;
                        header.OwnerId = short_header.OwnerId;
                        header.ObjectType = short_header.ObjectType;
                        header.CreationEpoch = short_header.CreationEpoch;
                        header.PayloadHash = short_header.PayloadHash;
                        header.HomomorphicHash = short_header.HomomorphicHash;
                        break;
                    }
                case HeadResponse.Types.Body.HeadOneofCase.Header:
                    {
                        if (request.Body.MainOnly) throw new FormatException("expect short header received full");
                        var full_header = resp.Body.Header;
                        if (full_header is null)
                            throw new FormatException("malformed object header");
                        header = full_header.Header;
                        sig = full_header.Signature;
                        if (!sig.VerifyMessagePart(request.Body.Address.ObjectId))
                        {
                            throw new InvalidOperationException(nameof(GetObjectHeader) + " invalid signature");
                        }
                        break;
                    }
                case HeadResponse.Types.Body.HeadOneofCase.SplitInfo:
                    {
                        throw new SplitInfoException(resp.Body.SplitInfo);
                    }
                default:
                    throw new FormatException("malformed object header response");
            }
            var obj = new Object.Object
            {
                ObjectId = request.Body.Address.ObjectId,
                Header = header,
                Signature = sig,
            };
            return obj;
        }

        public async Task<byte[]> GetObjectPayloadRangeData(Address address, Object.Range range, bool raw = false, CallOptions options = null, CancellationToken context = default)
        {
            if (address is null) throw new ArgumentException($"No Address in {nameof(address)}");
            if (range is null) throw new ArgumentException($"No Range in {nameof(range)}");
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var req = new GetRangeRequest
            {
                MetaHeader = opts.GetRequestMetaHeader(),
                Body = new GetRangeRequest.Types.Body
                {
                    Address = address,
                    Range = range,
                    Raw = raw,
                }
            };
            PrepareObjectSessionToken(req.MetaHeader, opts.Key, address, ObjectSessionContext.Types.Verb.Range);
            opts.Key.Sign(req);

            return await GetObjectPayloadRangeData(req, opts.Deadline, context);
        }

        public async Task<byte[]> GetObjectPayloadRangeData(GetRangeRequest request, DateTime? deadline = null, CancellationToken context = default)
        {
            var stream = ObjectClient.GetRange(request, deadline: deadline, cancellationToken: context).ResponseStream;
            var payload = new byte[request.Body.Range.Length];
            var offset = 0;
            while (await stream.MoveNext())
            {
                var resp = stream.Current;
                ProcessResponse(resp);
                resp.Body.Chunk.CopyTo(payload, offset);
                offset += resp.Body.Chunk.Length;
            }
            return payload;
        }

        public async Task<List<byte[]>> GetObjectPayloadRangeHash(Address address, IEnumerable<Object.Range> ranges, ChecksumType type, byte[] salt, CallOptions options = null, CancellationToken context = default)
        {
            if (address is null) throw new ArgumentNullException(nameof(address));
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var req = new GetRangeHashRequest
            {
                MetaHeader = opts.GetRequestMetaHeader(),
                Body = new GetRangeHashRequest.Types.Body
                {
                    Address = address,
                    Salt = salt is null ? ByteString.Empty : ByteString.CopyFrom(salt),
                    Type = type,
                }
            };
            req.Body.Ranges.AddRange(ranges);
            PrepareObjectSessionToken(req.MetaHeader, opts.Key, address, ObjectSessionContext.Types.Verb.Rangehash);
            opts.Key.Sign(req);

            return await GetObjectPayloadRangeHash(req, opts.Deadline, context);
        }

        public async Task<List<byte[]>> GetObjectPayloadRangeHash(GetRangeHashRequest request, DateTime? deadline = null, CancellationToken context = default)
        {
            var resp = await ObjectClient.GetRangeHashAsync(request, deadline: deadline, cancellationToken: context);
            ProcessResponse(resp);
            return resp.Body.HashList.Select(p => p.ToByteArray()).ToList();
        }

        public async Task<List<ObjectID>> SearchObject(ContainerID cid, SearchFilters filters, CallOptions options = null, CancellationToken context = default)
        {
            if (cid is null) throw new ArgumentNullException(nameof(cid));
            if (filters is null) throw new ArgumentNullException(nameof(filters));
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var req = new SearchRequest
            {
                MetaHeader = opts.GetRequestMetaHeader(),
                Body = new SearchRequest.Types.Body
                {
                    ContainerId = cid,
                    Version = SearchObjectVersion,
                }
            };
            req.Body.Filters.AddRange(filters.Filters);
            PrepareObjectSessionToken(req.MetaHeader, opts.Key, new Address { ContainerId = cid }, ObjectSessionContext.Types.Verb.Search);
            opts.Key.Sign(req);

            return await SearchObject(req, opts.Deadline, context);
        }

        public async Task<List<ObjectID>> SearchObject(SearchRequest request, DateTime? deadline = null, CancellationToken context = default)
        {
            var stream = ObjectClient.Search(request, deadline: deadline, cancellationToken: context).ResponseStream;
            var result = new List<ObjectID>();
            while (await stream.MoveNext())
            {
                var resp = stream.Current;
                ProcessResponse(resp);
                if (resp.Body?.IdList is not null)
                    result = result.Concat(resp.Body.IdList).ToList();
            }
            return result;
        }

        private void PrepareObjectSessionToken(RequestMetaHeader meta, ECDsa key, Address address, ObjectSessionContext.Types.Verb verb)
        {
            if (meta.SessionToken is null || meta.SessionToken.Signature != null)
                return;
            var ctx = new ObjectSessionContext
            {
                Address = address,
                Verb = verb,
            };
            meta.SessionToken.Body.Object = ctx;
            meta.SessionToken.Signature = key.SignMessagePart(meta.SessionToken.Body);
        }
    }
}
