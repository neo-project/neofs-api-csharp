// Copyright (C) 2015-2025 The Neo Project.
//
// Client.Accounting.cs file belongs to the neo project and is free
// software distributed under the MIT software license, see the
// accompanying file LICENSE in the main directory of the
// repository or http://www.opensource.org/licenses/mit-license.php
// for more details.
//
// Redistribution and use in source and binary forms with or without
// modifications are permitted.

using Neo.FileStorage.API.Accounting;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Refs;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Neo.FileStorage.API.Client
{
    public sealed partial class Client
    {
        public async Task<Accounting.Decimal> GetBalance(OwnerID owner = null, CallOptions options = null, CancellationToken context = default)
        {
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            if (owner is null) owner = opts.Key.OwnerID();
            var req = new BalanceRequest
            {
                Body = new BalanceRequest.Types.Body
                {
                    OwnerId = owner,
                },
                MetaHeader = opts.GetRequestMetaHeader()
            };
            opts.Key.Sign(req);
            return await GetBalance(req, opts.Deadline, context);
        }

        public async Task<Accounting.Decimal> GetBalance(BalanceRequest request, DateTime? deadline = null, CancellationToken context = default)
        {
            var resp = await AccountingClient.BalanceAsync(request, deadline: deadline, cancellationToken: context);
            ProcessResponse(resp);
            return resp.Body.Balance;
        }
    }
}
