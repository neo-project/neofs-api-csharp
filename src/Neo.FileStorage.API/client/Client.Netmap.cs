using System;
using System.Threading;
using System.Threading.Tasks;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Netmap;

namespace Neo.FileStorage.API.Client
{
    public sealed partial class Client
    {
        public async Task<NodeInfo> LocalNodeInfo(CallOptions options = null, CancellationToken context = default)
        {
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var req = new LocalNodeInfoRequest
            {
                Body = new LocalNodeInfoRequest.Types.Body { }
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
            opts.Key.Sign(req);

            return await LocalNodeInfo(req, opts.Deadline, context);
        }

        public async Task<NodeInfo> LocalNodeInfo(LocalNodeInfoRequest request, DateTime? deadline = null, CancellationToken context = default)
        {
            var resp = await NetmapClient.LocalNodeInfoAsync(request, deadline: deadline, cancellationToken: context);
            ProcessResponse(resp);
            return resp.Body.NodeInfo;
        }

        public async Task<ulong> Epoch(CallOptions options = null, CancellationToken context = default)
        {
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var req = new LocalNodeInfoRequest
            {
                MetaHeader = opts.GetRequestMetaHeader(),
                Body = new LocalNodeInfoRequest.Types.Body { }
            };
            opts.Key.Sign(req);

            return await Epoch(req, opts.Deadline, context);
        }

        public async Task<ulong> Epoch(LocalNodeInfoRequest request, DateTime? deadline = null, CancellationToken context = default)
        {
            var resp = await NetmapClient.LocalNodeInfoAsync(request, deadline: deadline, cancellationToken: context);
            ProcessResponse(resp);
            return resp.MetaHeader.Epoch;
        }

        public async Task<Refs.Version> Version(CallOptions options = null, CancellationToken context = default)
        {
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var req = new LocalNodeInfoRequest
            {
                MetaHeader = opts.GetRequestMetaHeader(),
                Body = new LocalNodeInfoRequest.Types.Body { }
            };
            opts.Key.Sign(req);

            return await Version(req, opts.Deadline, context);
        }

        public async Task<Refs.Version> Version(LocalNodeInfoRequest request, DateTime? deadline = null, CancellationToken context = default)
        {
            var resp = await NetmapClient.LocalNodeInfoAsync(request, deadline: deadline, cancellationToken: context);
            ProcessResponse(resp);
            return resp.Body.Version;
        }

        public async Task<NetworkInfo> NetworkInfo(CallOptions options = null, CancellationToken context = default)
        {
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var req = new NetworkInfoRequest
            {
                Body = new NetworkInfoRequest.Types.Body { }
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
            opts.Key.Sign(req);
            var resp = await NetmapClient.NetworkInfoAsync(req, cancellationToken: context);
            ProcessResponse(resp);
            return resp.Body.NetworkInfo;
        }
    }
}
