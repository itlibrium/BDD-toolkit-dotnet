using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITLIBRIUM.BddToolkit.Examples.Doubles
{
    public class FakePrePaidAccountHistoryDao : PrePaidAccountHistoryDao
    {
        private readonly Dictionary<Guid, PrePaidAccountHistory> _histories =
            new Dictionary<Guid, PrePaidAccountHistory>();

        public Task Append(Recharged recharged)
        {
            var accountId = recharged.PrePaidAccountId;
            if (!_histories.TryGetValue(accountId, out var history))
                _histories.Add(accountId, history = new PrePaidAccountHistory {AccountId = accountId});
            history.Operations.Add(new PrePaidAccountOperation
            {
                Type = nameof(Recharged),
                OccuredOn = recharged.OccuredOn,
                Amount = recharged.Value,
                CurrencyCode = recharged.CurrencyCode
            });
            return Task.CompletedTask;
        }

        public Task<PrePaidAccountHistory> GetBy(Guid accountId) => Task.FromResult(_histories[accountId]);
    }
}