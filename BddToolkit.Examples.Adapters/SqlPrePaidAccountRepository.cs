using System;
using System.Threading.Tasks;

namespace ITLIBRIUM.BddToolkit.Examples
{
    public class SqlPrePaidAccountRepository : PrePaidAccountRepository
    {
        // For now implementation is not needed because we don't have example of integration test
        public Task<PrePaidAccount> GetBy(PrePaidAccountId id) => throw new NotImplementedException();
        public Task Save(PrePaidAccount prePaidAccount) => throw new NotImplementedException();
    }
}