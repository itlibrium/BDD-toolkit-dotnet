using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentAssertions;
using ITLIBRIUM.BddToolkit.Examples.Doubles;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ITLIBRIUM.BddToolkit.Examples
{
    public class RechargePrePaidAccountTests
    {
        private Context CurrentContext { get; set; }

        
        [SetUp]
        public void SetUp()
        {
            var serviceProvider = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder => builder
                    .ConfigureServices(services => services
                        .AddScoped<PrePaidAccountRepository, FakePrePaidAccountRepository>()
                        .AddScoped<PrePaidAccountHistoryDao, FakePrePaidAccountHistoryDao>()
                        .AddScoped<Clock, FakeClock>()))
                .Services;
            CurrentContext = new Context(serviceProvider);
        }

        [TearDown]
        public void TearDown() => CurrentContext.Dispose();

        [Test]
        public Task VipRechargePolicyIsAppliedOnRecharge() => Bdd.Scenario(CurrentContext)
            .Given(c => c.AmountAvailableWas(100, Currency.PLN))
            .When(c => c.OwnerRechargesPrePaidAccountWith(50, Currency.PLN))
            .Then(c => c.AmountAvailableIs(160, Currency.PLN))
            .TestAsync();
        
        [Test]
        public Task EachOperationIsAppendedToHistory() => Bdd.Scenario(CurrentContext)
            .Given(c => c.NewAccountWasCreatedForCurrency(Currency.PLN))
            .And(c => c.AccountWasRechargedWith(120, Currency.PLN, "2020-11-13 13:13"))
            .And(c => c.AccountWasRechargedWith(240, Currency.PLN, "2020-11-14 14:14"))
            .Then(c => c.HistoryContainsOperations(
                    ("Recharged", 120, Currency.PLN, DateTime.Parse("2020-11-13 13:13")),
                    ("Recharged", 240, Currency.PLN, DateTime.Parse("2020-11-14 14:14"))),
                @"history contains operations: 
    1) Recharged 120.00 PLN on 2020-11-13:13:13
    2) Recharged 240.00 PLN on 2020-11-14:14:14")
            .TestAsync();

        private class Context : IDisposable
        {
            private readonly IServiceScope _scope;
            private readonly PrePaidAccountRepository _repository;
            private readonly CommandHandler<Recharge, Recharged> _rechargeHandler;
            private readonly PrePaidAccountHistoryDao _historyDao;
            private readonly FakeClock _clock;

            private PrePaidAccountId _id;

            public Context(IServiceProvider serviceProvider)
            {
                _scope = serviceProvider.CreateScope();
                _repository = _scope.ServiceProvider.GetService<PrePaidAccountRepository>();
                _rechargeHandler = _scope.ServiceProvider.GetService<CommandHandler<Recharge, Recharged>>();
                _historyDao = _scope.ServiceProvider.GetService<PrePaidAccountHistoryDao>();
                _clock = (FakeClock) _scope.ServiceProvider.GetService<Clock>();
            }

            public async Task AmountAvailableWas(decimal value, Currency currency)
            {
                var account = PrePaidAccount.New(currency);
                account.Recharge(Money.Of(value, currency));
                await _repository.Save(account);
                _id = account.Id;
            }

            public async Task NewAccountWasCreatedForCurrency(Currency currency)
            {
                var account = PrePaidAccount.New(currency);
                await _repository.Save(account);
                _id = account.Id;
            }

            public async Task AccountWasRechargedWith(decimal value, Currency currency, string occuredOn)
            {
                _clock.Now = DateTime.Parse(occuredOn);
                await _rechargeHandler.Handle(new Recharge(_id.Value, value / 1.2m, currency.ToString()));
            }

            public Task OwnerRechargesPrePaidAccountWith(decimal value, Currency currency) =>
                _rechargeHandler.Handle(new Recharge(_id.Value, value, currency.ToString()));

            public async Task AmountAvailableIs(decimal value, Currency currency)
            {
                var account = await _repository.GetBy(_id);
                var snapshot = account.GetSnapshot();
                snapshot.AmountAvailable.Value.Should().Be(value);
                snapshot.AmountAvailable.Currency.Should().Be(currency);
            }

            public async Task HistoryContainsOperations(params (string, decimal, Currency, DateTime)[] operations)
            {
                var history = await _historyDao.GetBy(_id.Value);
                history.Operations.Should().HaveCount(operations.Length);
                foreach (var (type, amount, currency, occuredOn) in operations)
                    history.Operations.Should().ContainSingle(
                        OperationWithValues(type, occuredOn, amount, currency));
            }

            private static Expression<Func<PrePaidAccountOperation, bool>> OperationWithValues(string type,
                DateTime occuredOn, decimal amount, Currency currency) =>
                operation => operation.Type == type &&
                             operation.OccuredOn == occuredOn &&
                             operation.Amount == amount &&
                             operation.CurrencyCode == currency.ToString();

            public void Dispose() => _scope?.Dispose();
        }
    }
}