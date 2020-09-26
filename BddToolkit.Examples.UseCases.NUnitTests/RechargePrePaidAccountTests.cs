using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ITLIBRIUM.BddToolkit.Examples
{
    public class RechargePrePaidAccountTests : IDisposable
    {
        private Context CurrentContext { get; set; }

        
        [SetUp]
        public void SetUp()
        {
            var serviceProvider = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder => builder
                    .ConfigureServices(services => services
                        .AddScoped<PrePaidAccountRepository, FakePrePaidAccountRepository>()))
                .Services;
            CurrentContext = new Context(serviceProvider);
        }

        [Test]
        public Task VipRechargePolicyIsAppliedOnRecharge() => Bdd.Scenario(CurrentContext)
            .Given(c => c.AmountAvailableWas(100, Currency.PLN))
            .When(c => c.OwnerRechargesPrePaidAccountWith(50, Currency.PLN))
            .Then(c => c.AmountAvailableIs(160, Currency.PLN))
            .TestAsync();

        private class Context : IDisposable
        {
            private readonly IServiceScope _scope;
            private readonly PrePaidAccountRepository _repository;
            private readonly CommandHandler<Recharge, Recharged> _rechargeHandler;
            
            private PrePaidAccountId _id;

            public Context(IServiceProvider serviceProvider)
            {
                _scope = serviceProvider.CreateScope();
                _repository = _scope.ServiceProvider.GetService<PrePaidAccountRepository>();
                _rechargeHandler = _scope.ServiceProvider.GetService<CommandHandler<Recharge, Recharged>>();
            }

            public async Task AmountAvailableWas(decimal value, Currency currency)
            {
                var account = PrePaidAccount.New(currency);
                account.Recharge(Money.Of(value, currency));
                await _repository.Save(account);
                _id = account.Id;
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

            public void Dispose() => _scope?.Dispose();
        }

        public void Dispose() => CurrentContext?.Dispose();
    }
}