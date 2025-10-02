// Copyright (C) 2015-2025 The Neo Project.
//
// Client.Session.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

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
                MetaHeader = opts?.GetRequestMetaHeader() ?? RequestMetaHeader.Default,
                Body = new CreateRequest.Types.Body
                {
                    OwnerId = opts.Key.OwnerID(),
                    Expiration = expiration,
                }
            };
            opts.Key.Sign(req);

            return await CreateSession(req, opts.Deadline, context);
        }

        public async Task<SessionToken> CreateSession(CreateRequest request, DateTime? deadline = null, CancellationToken context = default)
        {
            var resp = await SessionClient.CreateAsync(request, deadline: deadline, cancellationToken: context);
            ProcessResponse(resp);
            return new SessionToken
            {
                Body = new SessionToken.Types.Body
                {
                    Id = resp.Body.Id,
                    SessionKey = resp.Body.SessionKey,
                    OwnerId = request.Body.OwnerId,
                    Lifetime = new()
                    {
                        Exp = request.Body.Expiration,
                        Iat = resp.MetaHeader.Epoch,
                        Nbf = resp.MetaHeader.Epoch,
                    }
                }
            };
        }
    }
}
