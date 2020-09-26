using System;
using System.Threading.Tasks;

namespace ITLIBRIUM.BddToolkit.Examples
{
    public class RechargeHandler : CommandHandler<Recharge, Recharged>
    {
        private readonly PrePaidAccountRepository _repository;
        private readonly RechargePolicy _rechargePolicy;

        public RechargeHandler(PrePaidAccountRepository repository, RechargePolicy rechargePolicy)
        {
            _repository = repository;
            _rechargePolicy = rechargePolicy;
        }

        public async Task<Recharged> Handle(Recharge command)
        {
            var (id, amount) = CreateDomainModelFrom(command);
            amount = _rechargePolicy.CalculateAmount(amount);
            var prePaidAccount = await _repository.GetBy(id);
            prePaidAccount.Recharge(amount);
            await _repository.Save(prePaidAccount);
            return new Recharged(id.Value, amount.Value, amount.Currency.ToString());
        }

        private static (PrePaidAccountId, Money) CreateDomainModelFrom(Recharge command) => (
            PrePaidAccountId.Of(command.PrePaidAccountId),
            Money.Of(command.Value, Enum.Parse<Currency>(command.CurrencyCode, true)));

    }
}