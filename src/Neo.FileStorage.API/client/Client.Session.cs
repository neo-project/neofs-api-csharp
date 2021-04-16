using Neo.FileStorage.API.Acl;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Session;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Neo.FileStorage.API.Client
{
    public sealed partial class Client
    {
        public void AttachSessionToken(SessionToken token)
        {
            session = token;
        }

        public void AttachBearerToken(BearerToken token)
        {
            bearer = token;
        }

        public async Task<SessionToken> CreateSession(ulong expiration, CallOptions options = null, CancellationToken context = default)
        {
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            var req = new CreateRequest
            {
                Body = new CreateRequest.Types.Body
                {
                    OwnerId = opts.Key.ToOwnerID(),
                    Expiration = expiration,
                }
            };
            req.MetaHeader = opts?.GetRequestMetaHeader() ?? RequestMetaHeader.Default;
            opts.Key.SignRequest(req);

            var resp = await SessionClient.CreateAsync(req, cancellationToken: context);
            if (!resp.VerifyResponse())
                throw new FormatException("invalid balance response");
            return new SessionToken
            {
                Body = new SessionToken.Types.Body
                {
                    Id = resp.Body.Id,
                    SessionKey = resp.Body.SessionKey,
                    OwnerId = opts.Key.ToOwnerID(),
                }
            };
        }
    }
}
