// Copyright (C) 2015-2025 The Neo Project.
//
// Client.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Grpc.Core;
using Grpc.Net.Client;
using Neo.FileStorage.API.Accounting;
using Neo.FileStorage.API.Acl;
using Neo.FileStorage.API.Container;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Netmap;
using Neo.FileStorage.API.Object;
using Neo.FileStorage.API.Reputation;
using Neo.FileStorage.API.Session;
using System;
using System.Security.Cryptography;

namespace Neo.FileStorage.API.Client
{
    public sealed partial class Client : IFSClient, IFSRawClient
    {
        public const int DefaultConnectTimeoutMilliSeconds = 120000;
        public const uint SearchObjectVersion = 1;
        public const byte NeoAddressVersion = 0x35;

        private readonly ECDsa key;
        private readonly GrpcChannel channel;
        private SessionToken session;
        private BearerToken bearer;

        private AccountingService.AccountingServiceClient accountingClient = null;
        private ContainerService.ContainerServiceClient containerClient = null;
        private NetmapService.NetmapServiceClient netmapClient = null;
        private ObjectService.ObjectServiceClient objectClient = null;
        private SessionService.SessionServiceClient sessionClient = null;
        private ReputationService.ReputationServiceClient reputationClient = null;

        private AccountingService.AccountingServiceClient AccountingClient
        {
            get
            {
                if (accountingClient is null)
                    accountingClient = new AccountingService.AccountingServiceClient(channel);
                return accountingClient;
            }
        }

        private ContainerService.ContainerServiceClient ContainerClient
        {
            get
            {
                if (containerClient is null)
                    containerClient = new ContainerService.ContainerServiceClient(channel);
                return containerClient;
            }
        }

        private NetmapService.NetmapServiceClient NetmapClient
        {
            get
            {
                if (netmapClient is null)
                    netmapClient = new NetmapService.NetmapServiceClient(channel);
                return netmapClient;
            }
        }

        private ObjectService.ObjectServiceClient ObjectClient
        {
            get
            {
                if (objectClient is null)
                    objectClient = new ObjectService.ObjectServiceClient(channel);
                return objectClient;
            }
        }

        private SessionService.SessionServiceClient SessionClient
        {
            get
            {
                if (sessionClient is null)
                    sessionClient = new SessionService.SessionServiceClient(channel);
                return sessionClient;
            }
        }

        private ReputationService.ReputationServiceClient ReputationClient
        {
            get
            {
                if (reputationClient is null)
                    reputationClient = new ReputationService.ReputationServiceClient(channel);
                return reputationClient;
            }
        }

        public readonly string Host;

        /// <summary>
        /// Construct neofs client.
        /// </summary>
        /// <param name="key">ECDsa key to sign the request.</param>
        /// <param name="host">The url of neofs node, like: http://st2.storage.fs.neo.org:8080.</param>
        public Client(ECDsa key, string host)
        {
            if (host.StartsWith("https"))
                channel = GrpcChannel.ForAddress(host, new() { Credentials = new SslCredentials() });
            else
                channel = GrpcChannel.ForAddress(host, new() { Credentials = SslCredentials.Insecure });
            Host = host;
            this.key = key;
        }

        public void Dispose()
        {
            channel.ShutdownAsync().Wait();
            channel.Dispose();
        }

        public IFSRawClient Raw()
        {
            return this;
        }

        private CallOptions DefaultCallOptions => new()
        {
            Version = Refs.Version.SDKVersion(),
            Ttl = 2,
            Session = session,
            Bearer = bearer,
            Key = key,
        };

        private void CheckOptions(CallOptions options)
        {
            if (options.Key is null) throw new InvalidOperationException("missing sign key");
        }

        private void CheckStatus(IResponse resp)
        {
            var meta = resp.MetaHeader;
            if (meta?.Status is not null && !meta.Status.IsSuccess())
            {
                throw new RpcException(meta.Status.ToGrpcStatus());
            }
        }

        private void ProcessResponse(IResponse resp)
        {
            if (!resp.Verify())
                throw new FormatException($"invalid response, type={resp.GetType()}");
            CheckStatus(resp);
            if (responseHandler is not null)
                responseHandler(resp);
        }
    }
}
