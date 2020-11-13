using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITLIBRIUM.BddToolkit.Examples.Doubles
{
    public class FakePrePaidAccountRepository : PrePaidAccountRepository
    {
        private readonly Dictionary<PrePaidAccountId, PrePaidAccount> _accounts =
            new Dictionary<PrePaidAccountId, PrePaidAccount>();

        public Task<PrePaidAccount> GetBy(PrePaidAccountId id) => Task.FromResult(_accounts[id]);

        public Task Save(PrePaidAccount prePaidAccount)
        {
            _accounts[prePaidAccount.Id] = prePaidAccount;
            return Task.CompletedTask;
        }
    }
}