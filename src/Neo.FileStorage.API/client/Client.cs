using System;
using System.Security.Cryptography;
using Grpc.Core;
using Grpc.Net.Client;
using Neo.FileStorage.API.Accounting;
using Neo.FileStorage.API.Acl;
using Neo.FileStorage.API.Container;
using Neo.FileStorage.API.Netmap;
using Neo.FileStorage.API.Object;
using Neo.FileStorage.API.Reputation;
using Neo.FileStorage.API.Session;

namespace Neo.FileStorage.API.Client
{
    public sealed partial class Client : IFSClient, IFSRawClient
    {
        public const int DefaultConnectTimeoutMilliSeconds = 120000;
        public const uint SearchObjectVersion = 1;

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

        /// <summary>
        /// Construct neofs client.
        /// </summary>
        /// <param name="key">ECDsa key to sign the request.</param>
        /// <param name="host">The url of neofs node, like: http://st2.storage.fs.neo.org:8080.</param>
        public Client(ECDsa key, string host)
        {
            channel = GrpcChannel.ForAddress(host, new GrpcChannelOptions { Credentials = ChannelCredentials.Insecure });
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

        public CallOptions DefaultCallOptions
        {
            get
            {
                return new CallOptions
                {
                    Version = Refs.Version.SDKVersion(),
                    Ttl = 2,
                    Session = session,
                    Bearer = bearer,
                    Key = key,
                };
            }
        }

        private void CheckOptions(CallOptions options)
        {
            if (options.Key is null) throw new InvalidOperationException("missing sign key");
        }
    }
}
