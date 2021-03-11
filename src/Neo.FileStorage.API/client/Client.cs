using Grpc.Core;
using Neo.FileStorage.API.Acl;
using Neo.FileStorage.API.Refs;
using Neo.FileStorage.API.Session;
using System;
using System.Security.Cryptography;

namespace Neo.FileStorage.API.Client
{
    public partial class Client
    {
        public const int DefaultConnectTimeoutMilliSeconds = 120000;
        const uint SearchObjectVersion = 1;
        private readonly ECDsa key;
        private readonly Channel channel;
        private SessionToken session;
        private BearerToken bearer;

        public Client(ECDsa key, string host, int milliSecondTimeout = DefaultConnectTimeoutMilliSeconds)
        {
            channel = new Channel(host, ChannelCredentials.Insecure, new ChannelOption[] { new ChannelOption("grpc.server_handshake_timeout_ms", milliSecondTimeout) });
            this.key = key;
        }

        public void Close()
        {
            channel.ShutdownAsync().Wait();
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
                };
            }
        }
    }
}
