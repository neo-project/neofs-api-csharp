using System;
using System.Threading;
using System.Threading.Tasks;
using Neo.FileStorage.API.Acl;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Refs;
using Neo.FileStorage.API.Session;

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
                    OwnerId = OwnerID.FromScriptHash(opts.Key.PublicKey().PublicKeyToScriptHash()),
                    Expiration = expiration,
                }
            };
            req.MetaHeader = opts?.GetRequestMetaHeader() ?? RequestMetaHeader.Default;
            opts.Key.SignRequest(req);

            return await CreateSession(req, opts.Deadline, context);
        }

        public async Task<SessionToken> CreateSession(CreateRequest request, DateTime? deadline = null, CancellationToken context = default)
        {
            var resp = await SessionClient.CreateAsync(request, deadline: deadline, cancellationToken: context);
            if (!resp.Verify())
                throw new FormatException("invalid session response");
            CheckStatus(resp);
            return new SessionToken
            {
                Body = new SessionToken.Types.Body
                {
                    Id = resp.Body.Id,
                    SessionKey = resp.Body.SessionKey,
                    OwnerId = request.Body.OwnerId
                }
            };
        }
    }
}
