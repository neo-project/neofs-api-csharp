using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Neo.FileStorage.API.Control
{
    public class ControlClient
    {
        private readonly GrpcChannel channel;
        private readonly ECDsa key;
        private readonly ControlService.ControlServiceClient controlServiceClient;

        public ControlClient(ECDsa key, string host)
        {
            if (host.StartsWith("https"))
                channel = GrpcChannel.ForAddress(host, new() { Credentials = new SslCredentials() });
            else
                channel = GrpcChannel.ForAddress(host, new() { Credentials = SslCredentials.Insecure });
            this.key = key;
            controlServiceClient = new ControlService.ControlServiceClient(channel);
        }

        public void DropObjects(params ByteString[] addresses)
        {
            var request = new DropObjectsRequest
            {
                Body = new()
            };
            request.Body.AddressList.AddRange(addresses);
            key.SignControlMessage(request);
            var resp = controlServiceClient.DropObjects(request);
            if (!resp.VerifyControlMessage())
                throw new InvalidOperationException("invalid drop objets response");
        }

        public HealthStatus HealthCheck()
        {
            var request = new HealthCheckRequest
            {
                Body = new()
            };
            key.SignControlMessage(request);
            var resp = controlServiceClient.HealthCheck(request);
            if (!resp.VerifyControlMessage())
                throw new InvalidOperationException("invalid health check response");
            return resp.Body.HealthStatus;
        }

        public Netmap NetmapSnapshot()
        {
            var request = new NetmapSnapshotRequest
            {
                Body = new()
            };
            key.SignControlMessage(request);
            var resp = controlServiceClient.NetmapSnapshot(request);
            if (!resp.VerifyControlMessage())
                throw new InvalidOperationException("invalid netmap snapshot response");
            return resp.Body.Netmap;
        }

        public void SetNetmapStatusRequest(NetmapStatus status)
        {
            var request = new SetNetmapStatusRequest
            {
                Body = new()
                {
                    Status = status,
                }
            };
            key.SignControlMessage(request);
            var resp = controlServiceClient.SetNetmapStatus(request);
            if (!resp.VerifyControlMessage())
                throw new InvalidOperationException("invalid drop objets response");
        }
    }
}
