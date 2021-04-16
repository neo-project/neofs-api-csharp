using Neo.FileStorage.API.Acl;
using Neo.FileStorage.API.Container;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Refs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UsedSpaceAnnouncement = Neo.FileStorage.API.Container.AnnounceUsedSpaceRequest.Types.Body.Types.Announcement;

namespace Neo.FileStorage.API.Client
{
    public sealed partial class Client
    {
        public async Task<Container.Container> GetContainer(ContainerID cid, CallOptions options = null, CancellationToken context = default)
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
            opts.Key.SignRequest(req);

            var resp = await ContainerClient.GetAsync(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid container get response");
            return resp.Body.Container;
        }

        public async Task<ContainerID> PutContainer(Container.Container container, CallOptions options = null, CancellationToken context = default)
        {
            if (container is null) throw new ArgumentNullException(nameof(container));
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            container.Version = Refs.Version.SDKVersion();
            if (container.OwnerId is null) container.OwnerId = opts.Key.ToOwnerID();
            var req = new PutRequest
            {
                Body = new PutRequest.Types.Body
                {
                    Container = container,
                    Signature = opts.Key.SignRFC6979(container),
                }
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
            opts.Key.SignRequest(req);
            var resp = await ContainerClient.PutAsync(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid container put response");
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
            body.Signature = opts.Key.SignRFC6979(body);
            req.Body = body;
            req.MetaHeader = opts.GetRequestMetaHeader();
            opts.Key.SignRequest(req);

            var resp = await ContainerClient.DeleteAsync(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid container put response");
        }

        public async Task<List<ContainerID>> ListContainers(OwnerID owner = null, CallOptions options = null, CancellationToken context = default)
        {
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            if (owner is null) opts.Key.ToOwnerID();
            var req = new ListRequest
            {
                Body = new ListRequest.Types.Body
                {
                    OwnerId = owner
                }
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
            opts.Key.SignRequest(req);

            var resp = await ContainerClient.ListAsync(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid container put response");
            return resp.Body.ContainerIds.ToList();
        }

        public async Task<EAclWithSignature> GetEAclWithSignature(ContainerID cid, CallOptions options = null, CancellationToken context = default)
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
            opts.Key.SignRequest(req);

            var resp = await ContainerClient.GetExtendedACLAsync(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid container put response");
            var eacl = resp.Body.Eacl;
            var sig = resp.Body.Signature;
            return new EAclWithSignature
            {
                Table = eacl,
                Signature = sig,
            };
        }

        public async Task<EACLTable> GetEACL(ContainerID cid, CallOptions options = null, CancellationToken context = default)
        {
            if (cid is null) throw new ArgumentNullException(nameof(cid));
            var eacl_with_sig = await GetEAclWithSignature(cid, options, context);
            if (!eacl_with_sig.Signature.VerifyRFC6979(eacl_with_sig.Table))
                throw new InvalidOperationException("invalid eacl signature");
            return eacl_with_sig.Table;
        }

        public async Task SetEACL(EACLTable eacl, CallOptions options = null, CancellationToken context = default)
        {
            if (eacl is null) throw new ArgumentNullException(nameof(eacl));
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            eacl.Version = Refs.Version.SDKVersion();
            var req = new SetExtendedACLRequest
            {
                Body = new SetExtendedACLRequest.Types.Body
                {
                    Eacl = eacl,
                    Signature = opts.Key.SignRFC6979(eacl),
                }
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
            opts.Key.SignRequest(req);

            var resp = await ContainerClient.SetExtendedACLAsync(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid container put response");
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
            opts.Key.SignRequest(req);

            var resp = await ContainerClient.AnnounceUsedSpaceAsync(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new InvalidOperationException("invalid announce used space response");
        }
    }
}
