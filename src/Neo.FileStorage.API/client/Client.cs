using Grpc.Core;
using Grpc.Net.Client;
using Neo.FileStorage.API.Acl;
using Neo.FileStorage.API.Refs;
using Neo.FileStorage.API.Session;
using System;
using System.Security.Cryptography;

namespace Neo.FileStorage.API.Client
{
    public sealed partial class Client : IDisposable
    {
        public const int DefaultConnectTimeoutMilliSeconds = 120000;
        const uint SearchObjectVersion = 1;
        private readonly ECDsa key;
        private readonly GrpcChannel channel;
        private SessionToken session;
        private BearerToken bearer;

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
