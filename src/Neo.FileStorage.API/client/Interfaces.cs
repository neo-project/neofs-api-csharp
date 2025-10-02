// Copyright (C) 2015-2025 The Neo Project.
//
// Interfaces.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.FileStorage.API.Accounting;
using Neo.FileStorage.API.Acl;
using Neo.FileStorage.API.Container;
using Neo.FileStorage.API.Netmap;
using Neo.FileStorage.API.Object;
using Neo.FileStorage.API.Refs;
using Neo.FileStorage.API.Reputation;
using Neo.FileStorage.API.Session;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UsedSpaceAnnouncement = Neo.FileStorage.API.Container.AnnounceUsedSpaceRequest.Types.Body.Types.Announcement;

namespace Neo.FileStorage.API.Client
{
    public interface IFSClient : IAccountingClient, IContainerClient, IObjectClient, IReputationClient, INetmapClient, ISessionClient, IDisposable
    {
        IFSRawClient Raw();
    }

    public interface IFSRawClient : IRawAccountingClient, IRawContainerClient, IRawObjectClient, IRawNetmapClient, IRawReputationClient,
     IRawSessionClient
    { }

    public interface IAccountingClient
    {
        Task<Accounting.Decimal> GetBalance(OwnerID owner, CallOptions options = null, CancellationToken context = default);
    }

    public interface IRawAccountingClient
    {
        Task<Accounting.Decimal> GetBalance(BalanceRequest request, DateTime? deadline = null, CancellationToken context = default);
    }

    public interface IContainerClient
    {
        Task<ContainerWithSignature> GetContainer(ContainerID cid, CallOptions options = null, CancellationToken context = default);
        Task<ContainerID> PutContainer(Container.Container container, CallOptions options = null, CancellationToken context = default);
        Task DeleteContainer(ContainerID cid, CallOptions options = null, CancellationToken context = default);
        Task<List<ContainerID>> ListContainers(OwnerID owner, CallOptions options = null, CancellationToken context = default);
        Task<EAclWithSignature> GetEAcl(ContainerID cid, CallOptions options = null, CancellationToken context = default);
        Task SetEACL(EACLTable eacl, CallOptions options = null, CancellationToken context = default);
        Task AnnounceContainerUsedSpace(List<UsedSpaceAnnouncement> announcements, CallOptions options = null, CancellationToken context = default);
    }

    public interface IRawContainerClient
    {
        Task<ContainerWithSignature> GetContainer(Container.GetRequest request, DateTime? deadline = null, CancellationToken context = default);
        Task<ContainerID> PutContainer(Container.PutRequest request, DateTime? deadline = null, CancellationToken context = default);
        Task DeleteContainer(Container.DeleteRequest request, DateTime? deadline = null, CancellationToken context = default);
        Task<List<ContainerID>> ListContainers(ListRequest request, DateTime? deadline = null, CancellationToken context = default);
        Task<EAclWithSignature> GetEAcl(GetExtendedACLRequest request, DateTime? deadline = null, CancellationToken context = default);
        Task SetEACL(SetExtendedACLRequest request, DateTime? deadline = null, CancellationToken context = default);
        Task AnnounceContainerUsedSpace(AnnounceUsedSpaceRequest request, DateTime? deadline = null, CancellationToken context = default);
    }

    public interface INetmapClient
    {
        Task<NodeInfo> LocalNodeInfo(CallOptions options = null, CancellationToken context = default);
        Task<ulong> Epoch(CallOptions options = null, CancellationToken context = default);
        Task<NetworkInfo> NetworkInfo(CallOptions options = null, CancellationToken context = default);
    }

    public interface IRawNetmapClient
    {
        Task<NodeInfo> LocalNodeInfo(LocalNodeInfoRequest request, DateTime? deadline = null, CancellationToken context = default);
        Task<ulong> Epoch(LocalNodeInfoRequest request, DateTime? deadline = null, CancellationToken context = default);
    }

    public interface IObjectClient : IObjectDeleteClient, IObjectPutClient, IObjectGetClient, IObjectSearchClient { }

    public interface IObjectDeleteClient
    {
        Task<Address> DeleteObject(Address address, CallOptions options = null, CancellationToken context = default);
    }

    public interface IObjectPutClient
    {
        Task<ObjectID> PutObject(Object.Object obj, CallOptions options = null, CancellationToken context = default);
    }

    public interface IObjectGetClient
    {
        Task<Object.Object> GetObject(Address address, bool raw = false, CallOptions options = null, CancellationToken context = default);
        Task<Object.Object> GetObjectHeader(Address address, bool minimal = false, bool raw = false, CallOptions options = null, CancellationToken context = default);
        Task<byte[]> GetObjectPayloadRangeData(Address address, Object.Range range, bool raw = false, CallOptions options = null, CancellationToken context = default);
        Task<List<byte[]>> GetObjectPayloadRangeHash(Address address, IEnumerable<Object.Range> ranges, ChecksumType type, byte[] salt, CallOptions options = null, CancellationToken context = default);
    }

    public interface IObjectSearchClient
    {
        Task<List<ObjectID>> SearchObject(ContainerID cid, SearchFilters filters, CallOptions options = null, CancellationToken context = default);
    }

    public interface IRawObjectClient : IRawObjectGetClient, IRawObjectPutClient, IRawObjectSearchClient, IRawObjectDeleteClient { }

    public interface IRawObjectGetClient
    {
        Task<Object.Object> GetObject(Object.GetRequest request, DateTime? deadline = null, CancellationToken context = default);
        Task<Object.Object> GetObjectHeader(HeadRequest request, DateTime? deadline = null, CancellationToken context = default);
        Task<byte[]> GetObjectPayloadRangeData(GetRangeRequest request, DateTime? deadline = null, CancellationToken context = default);
        Task<List<byte[]>> GetObjectPayloadRangeHash(GetRangeHashRequest request, DateTime? deadline = null, CancellationToken context = default);
    }

    public interface IRawObjectPutClient
    {
        Task<IClientStream> PutObject(Object.PutRequest init, DateTime? deadline = null, CancellationToken context = default);
    }

    public interface IRawObjectSearchClient
    {
        Task<List<ObjectID>> SearchObject(SearchRequest request, DateTime? deadline = null, CancellationToken context = default);
    }

    public interface IRawObjectDeleteClient
    {
        Task<Address> DeleteObject(Object.DeleteRequest request, DateTime? deadline = null, CancellationToken context = default);
    }

    public interface IReputationClient
    {
        Task AnnounceTrust(ulong epoch, List<Trust> trusts, CallOptions options = null, CancellationToken context = default);
        Task AnnounceIntermediateTrust(ulong epoch, uint iter, PeerToPeerTrust trust, CallOptions options = null, CancellationToken context = default);
    }

    public interface IRawReputationClient
    {
        Task AnnounceTrust(AnnounceLocalTrustRequest request, DateTime? deadline = null, CancellationToken context = default);
        Task AnnounceIntermediateTrust(AnnounceIntermediateResultRequest request, DateTime? deadline = null, CancellationToken context = default);
    }

    public interface ISessionClient
    {
        Task<SessionToken> CreateSession(ulong expiration, CallOptions options = null, CancellationToken context = default);
    }

    public interface IRawSessionClient
    {
        Task<SessionToken> CreateSession(CreateRequest request, DateTime? deadline = null, CancellationToken context = default);
    }
}
