using System.Threading.Tasks;

namespace ITLIBRIUM.BddToolkit.Examples
{
    public interface PrePaidAccountRepository
    {
        Task<PrePaidAccount> GetBy(PrePaidAccountId id);
        Task Save(PrePaidAccount prePaidAccount);
    }
}