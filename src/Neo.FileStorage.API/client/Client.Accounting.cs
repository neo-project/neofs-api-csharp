using Neo.FileStorage.API.Accounting;
using Neo.FileStorage.API.Cryptography;
using Neo.FileStorage.API.Refs;
using System;
using System.Threading.Tasks;

namespace Neo.FileStorage.API.Client
{
    public sealed partial class Client
    {
        public async Task<Accounting.Decimal> GetBalance(OwnerID owner = null, CallOptions options = null)
        {
            var account_client = new AccountingService.AccountingServiceClient(channel);
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            CheckOptions(opts);
            if (owner is null) owner = opts.Key.ToOwnerID();
            var req = new BalanceRequest
            {
                Body = new BalanceRequest.Types.Body
                {
                    OwnerId = owner,
                }
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
            opts.Key.SignRequest(req);
            var resp = await account_client.BalanceAsync(req);
            if (!resp.VerifyResponse())
                throw new FormatException("invalid balance response");
            return resp.Body.Balance;
        }
    }
}
