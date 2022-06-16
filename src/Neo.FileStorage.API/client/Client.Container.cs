using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Neo.FileStorage.API.Acl;
using Neo.FileStorage.API.Container;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Refs;
using UsedSpaceAnnouncement = Neo.FileStorage.API.Container.AnnounceUsedSpaceRequest.Types.Body.Types.Announcement;

namespace Neo.FileStorage.API.Client
{
    public sealed partial class Client
    {
        public async Task<ContainerWithSignature> GetContainer(ContainerID cid, CallOptions options = null, CancellationToken context = default)
        {
            if (cid is null) throw new ArgumentNullException(nameof(cid));
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var req = new GetRequest
            {
                Body = new GetRequest.Types.Body
                {
                    ContainerId = cid
                }
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
            opts.Key.Sign(req);

            return await GetContainer(req, opts.Deadline, context);
        }

        public async Task<ContainerWithSignature> GetContainer(GetRequest request, DateTime? deadline = null, CancellationToken context = default)
        {
            var resp = await ContainerClient.GetAsync(request, deadline: deadline, cancellationToken: context);
            ProcessResponse(resp);
            if (!resp.Body.Signature.VerifyRFC6979(resp.Body.Container))
                throw new InvalidOperationException("invalid container signature");
            return new()
            {
                Container = resp.Body.Container,
                Signature = resp.Body.Signature,
                SessionToken = resp.Body.SessionToken
            };
        }

        public async Task<ContainerID> PutContainer(Container.Container container, CallOptions options = null, CancellationToken context = default)
        {
            if (container is null) throw new ArgumentNullException(nameof(container));
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            container.Version = Refs.Version.SDKVersion();
            if (container.OwnerId is null) container.OwnerId = opts.Key.OwnerID();
            var req = new PutRequest
            {
                Body = new PutRequest.Types.Body
                {
                    Container = container,
                    Signature = opts.Key.SignRFC6979(container),
                }
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
            opts.Key.Sign(req);

            return await PutContainer(req, opts.Deadline, context);
        }

        public async Task<ContainerID> PutContainer(PutRequest request, DateTime? deadline = null, CancellationToken context = default)
        {
            var resp = await ContainerClient.PutAsync(request, deadline: deadline, cancellationToken: context);
            ProcessResponse(resp);
            return resp.Body.ContainerId;
        }

        public async Task DeleteContainer(ContainerID cid, CallOptions options = null, CancellationToken context = default)
        {
            if (cid is null) throw new ArgumentNullException(nameof(cid));
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var body = new DeleteRequest.Types.Body
            {
                ContainerId = cid,
            };
            var req = new DeleteRequest();
            body.Signature = new()
            {
                Key = ByteString.CopyFrom(key.PublicKey()),
                Sign = ByteString.CopyFrom(opts.Key.SignRFC6979(cid.Value.ToByteArray())),
            };
            req.Body = body;
            req.MetaHeader = opts.GetRequestMetaHeader();
            opts.Key.Sign(req);

            await DeleteContainer(req, opts.Deadline, context);
        }

        public async Task DeleteContainer(DeleteRequest request, DateTime? deadline = null, CancellationToken context = default)
        {
            var resp = await ContainerClient.DeleteAsync(request, deadline: deadline, cancellationToken: context);
            ProcessResponse(resp);
        }

        public async Task<List<ContainerID>> ListContainers(OwnerID owner = null, CallOptions options = null, CancellationToken context = default)
        {
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            if (owner is null) owner = opts.Key.OwnerID();
            var req = new ListRequest
            {
                Body = new ListRequest.Types.Body
                {
                    OwnerId = owner
                }
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
            opts.Key.Sign(req);

            return await ListContainers(req, opts.Deadline, context);
        }

        public async Task<List<ContainerID>> ListContainers(ListRequest request, DateTime? deadline = null, CancellationToken context = default)
        {
            var resp = await ContainerClient.ListAsync(request, deadline: deadline, cancellationToken: context);
            ProcessResponse(resp);
            return resp.Body.ContainerIds.ToList();
        }

        public async Task<EAclWithSignature> GetEAcl(ContainerID cid, CallOptions options = null, CancellationToken context = default)
        {
            if (cid is null) throw new ArgumentNullException(nameof(cid));
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var req = new GetExtendedACLRequest
            {
                Body = new GetExtendedACLRequest.Types.Body
                {
                    ContainerId = cid
                }
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
            opts.Key.Sign(req);

            return await GetEAcl(req, opts.Deadline, context);
        }

        public async Task<EAclWithSignature> GetEAcl(GetExtendedACLRequest request, DateTime? deadline = null, CancellationToken context = default)
        {
            var resp = await ContainerClient.GetExtendedACLAsync(request, deadline: deadline, cancellationToken: context);
            ProcessResponse(resp);
            if (!resp.Body.Signature.VerifyRFC6979(resp.Body.Eacl))
                throw new InvalidOperationException("invalid eacl signature");
            return new EAclWithSignature
            {
                Table = resp.Body.Eacl,
                Signature = resp.Body.Signature,
                SessionToken = resp.Body.SessionToken
            };
        }

        public async Task SetEACL(EACLTable eacl, CallOptions options = null, CancellationToken context = default)
        {
            if (eacl is null) throw new ArgumentNullException(nameof(eacl));
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            eacl.Version = Refs.Version.SDKVersion();
            var req = new SetExtendedACLRequest
            {
                MetaHeader = opts.GetRequestMetaHeader(),
                Body = new SetExtendedACLRequest.Types.Body
                {
                    Eacl = eacl,
                    Signature = opts.Key.SignRFC6979(eacl),
                }
            };
            opts.Key.Sign(req);

            await SetEACL(req, opts.Deadline, context);
        }

        public async Task SetEACL(SetExtendedACLRequest request, DateTime? deadline = null, CancellationToken context = default)
        {
            var resp = await ContainerClient.SetExtendedACLAsync(request, deadline: deadline, cancellationToken: context);
            ProcessResponse(resp);
        }

        public async Task AnnounceContainerUsedSpace(List<UsedSpaceAnnouncement> announcements, CallOptions options = null, CancellationToken context = default)
        {
            if (announcements is null) throw new ArgumentNullException(nameof(announcements));
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var body = new AnnounceUsedSpaceRequest.Types.Body();
            body.Announcements.AddRange(announcements);
            var req = new AnnounceUsedSpaceRequest
            {
                Body = body,
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
            opts.Key.Sign(req);

            await AnnounceContainerUsedSpace(req, opts.Deadline, context);
        }

        public async Task AnnounceContainerUsedSpace(AnnounceUsedSpaceRequest request, DateTime? deadline = null, CancellationToken context = default)
        {
            var resp = await ContainerClient.AnnounceUsedSpaceAsync(request, deadline: deadline, cancellationToken: context);
            ProcessResponse(resp);
        }
    }
}
