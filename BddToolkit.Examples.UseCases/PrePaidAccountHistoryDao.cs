using System;
using System.Threading.Tasks;

namespace ITLIBRIUM.BddToolkit.Examples
{
    public interface PrePaidAccountHistoryDao
    {
        Task Append(Recharged recharged);
        public Task<PrePaidAccountHistory> GetBy(Guid accountId);
    }
}