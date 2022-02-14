using System;
using System.Threading;
using System.Threading.Tasks;
using Neo.FileStorage.API.Accounting;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Refs;

namespace Neo.FileStorage.API.Client
{
    public sealed partial class Client
    {
        public async Task<Accounting.Decimal> GetBalance(OwnerID owner = null, CallOptions options = null, CancellationToken context = default)
        {
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            if (owner is null) owner = OwnerID.FromScriptHash(opts.Key.PublicKey().PublicKeyToScriptHash());
            var req = new BalanceRequest
            {
                Body = new BalanceRequest.Types.Body
                {
                    OwnerId = owner,
                }
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
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
