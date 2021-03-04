using Neo.FileSystem.API.Accounting;
using Neo.FileSystem.API.Cryptography;
using Neo.FileSystem.API.Refs;
using System;

namespace Neo.FileSystem.API.Client
{
    public partial class Client
    {
        public Accounting.Decimal GetBalance(OwnerID owner, CallOptions options = null)
        {
            var account_client = new AccountingService.AccountingServiceClient(channel);
            var opts = DefaultCallOptions.ApplyCustomOptions(options);
            var req = new BalanceRequest
            {
                Body = new BalanceRequest.Types.Body
                {
                    OwnerId = owner,
                }
            };
            req.MetaHeader = opts.GetRequestMetaHeader();
            key.SignRequest(req);
            var resp = account_client.Balance(req);
            if (!resp.VerifyResponse())
                throw new FormatException("invalid balance response");
            return resp.Body.Balance;
        }

        public Accounting.Decimal GetSelfBalance()
        {
            var w = key.ToOwnerID();
            return GetBalance(w);
        }
    }
}
