using System;
using System.Threading.Tasks;

namespace ITLIBRIUM.BddToolkit.Examples
{
    public class RechargeHandler : CommandHandler<Recharge, Recharged>
    {
        private readonly PrePaidAccountRepository _repository;
        private readonly RechargePolicy _rechargePolicy;
        private readonly PrePaidAccountHistoryDao _historyDao;
        private readonly Clock _clock;

        public RechargeHandler(PrePaidAccountRepository repository, RechargePolicy rechargePolicy,
            PrePaidAccountHistoryDao historyDao, Clock clock)
        {
            _repository = repository;
            _rechargePolicy = rechargePolicy;
            _historyDao = historyDao;
            _clock = clock;
        }

        public async Task<Recharged> Handle(Recharge command)
        {
            var (id, amount) = CreateDomainModelFrom(command);
            amount = _rechargePolicy.CalculateAmount(amount);
            var prePaidAccount = await _repository.GetBy(id);
            prePaidAccount.Recharge(amount);
            await _repository.Save(prePaidAccount);
            var recharged = new Recharged(id.Value, _clock.Now, amount.Value, amount.Currency.ToString());
            await _historyDao.Append(recharged);
            return recharged;
        }

        private static (PrePaidAccountId, Money) CreateDomainModelFrom(Recharge command) => (
            PrePaidAccountId.Of(command.PrePaidAccountId),
            Money.Of(command.Value, Enum.Parse<Currency>(command.CurrencyCode, true)));
    }
}