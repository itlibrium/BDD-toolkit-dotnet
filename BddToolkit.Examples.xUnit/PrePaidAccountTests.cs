using FluentAssertions;
using ITLIBRIUM.BddToolkit.FluentAssertions;
using ITLIBRIUM.BddToolkit.Syntax.Features;
using ITLIBRIUM.BddToolkit.Syntax.Rules;
using Xunit;

namespace ITLIBRIUM.BddToolkit.Examples
{
    public class PrePaidAccountTests
    {
        //Given section is optional.
        [Fact]
        public void NewAccountHasNoAmountAvailable() => Bdd.Scenario<Context>()
            .When(c => c.NewAccountIsCreated(Currency.PLN))
            .Then(c => c.AmountAvailableIs(0, Currency.PLN))
            .Test();

        //Given and Then sections can have single element.
        [Fact]
        public void CanPayUpToAmountAvailable() => Bdd.Scenario<Context>()
            .Given(c => c.AmountAvailableWas(100, Currency.PLN))
            .When(c => c.AccountIsCharged(10, Currency.PLN))
            .Then(c => c.AmountAvailableIs(90, Currency.PLN))
            .Test();

        //Given and Then sections can have multiple elements.
        [Fact]
        public void CanPayUpToSumOfAmountAvailableAndDebtLimit() => Bdd.Scenario<Context>()
            .Given(c => c.AmountAvailableWas(10, Currency.PLN))
            .And(c => c.DebtLimitWas(100, Currency.PLN))
            .And(c => c.DebtWas(0, Currency.PLN))
            .When(c => c.AccountIsCharged(20, Currency.PLN))
            .Then(c => c.AmountAvailableIs(0, Currency.PLN))
            .And(c => c.DebtIs(10, Currency.PLN))
            .Test();

        //Exceptions can be checked using Result argument in lambda expression passed to Then method.
        [Fact]
        public void CanNotPayOverAmountAvailable() => Bdd.Scenario<Context>()
            .Given(c => c.AmountAvailableWas(100, Currency.PLN))
            .And(c => c.DebtLimitWas(0, Currency.PLN))
            .When(c => c.AccountIsCharged(101, Currency.PLN))
            .Then((c, r) => c.PaymentIsRefused(r))
            .Test();

        //Exceptions can be checked using Then() method and Throws<T>() or ThrowsExactly<T>() extension.
        //Reference to BddToolkit.FluentAssertion or BddToolkit.Shouldly is required.
        [Fact]
        public void CanNotPayOverDebtLimit() => Bdd.Scenario<Context>()
            .Given(c => c.AmountAvailableWas(0, Currency.PLN))
            .And(c => c.DebtLimitWas(100, Currency.PLN))
            .And(c => c.DebtWas(0, Currency.PLN))
            .When(c => c.AccountIsCharged(101, Currency.PLN))
            .Then().Throws<DomainException>()
            .Test();

        //Error checks and normal assertions can be mixed.
        [Fact]
        public void AmountIsUnchangedIfPaymentIsRefused() => Bdd.Scenario<Context>()
            .Given(c => c.AmountAvailableWas(100, Currency.PLN))
            .And(c => c.DebtLimitWas(0, Currency.PLN))
            .When(c => c.AccountIsCharged(101, Currency.PLN))
            .Then((c, r) => c.PaymentIsRefused(r))
            .And(c => c.AmountAvailableIs(100, Currency.PLN))
            .Test();

        //Name of the scenario can be passed explicitly. In default behaviour it's taken from test method name.
        //Also additional description can be added. Adding name is not required to add description.
        //Scenario can by tagged. Adding name or description is not required to add tags.
        [Fact]
        public void DebtIsUnchangedIfPaymentIsRefused() => Bdd.Scenario<Context>()
            .Name("Name of the scenario other then test method name")
            .Description("Additional description if something more than the name is needed")
            .Tags("tag1", "tag2", "tag3")
            .Given(c => c.AmountAvailableWas(0, Currency.PLN))
            .And(c => c.DebtLimitWas(100, Currency.PLN))
            .And(c => c.DebtWas(50, Currency.PLN))
            .When(c => c.AccountIsCharged(51, Currency.PLN))
            .Then((c, r) => c.PaymentIsRefused(r))
            .And(c => c.DebtIs(50, Currency.PLN))
            .Test();

        //Features can be declared to group scenarios and put feature names into the code.
        //Feature can be tagged. Adding description is not required to add tags.
        private static readonly Feature RechargingPrePaidAccount = Bdd.Feature(nameof(RechargingPrePaidAccount))
            .Description("Optional description")
            .Tags("tag1", "tag2");

        //Feature can be passed to the scenario.
        [Fact]
        public void RechargingIncreasesAmountAvailable() => Bdd.Scenario<Context>()
            .Feature(RechargingPrePaidAccount)
            .Given(c => c.AmountAvailableWas(100, Currency.PLN))
            .When(c => c.AccountIsRecharged(10, Currency.PLN))
            .Then(c => c.AmountAvailableIs(110, Currency.PLN))
            .Test();

        //Rules can be declared to group scenarios inside feature and put rule names into the code. 
        private static readonly Rule DebtIsAlwaysRepaidInTheFirstPlace = Bdd
            .Rule(nameof(DebtIsAlwaysRepaidInTheFirstPlace))
            .Feature(RechargingPrePaidAccount)
            .Description("Optional description");

        //Rule can be passed to the scenario.
        //If rule is passed then feature can't be passed because it's taken from the rule.
        [Fact]
        public void DebtIsRepaidBeforeAmountAvailableIsIncreased() => Bdd.Scenario<Context>()
            .Rule(DebtIsAlwaysRepaidInTheFirstPlace)
            .Given(c => c.AmountAvailableWas(0, Currency.PLN))
            .And(c => c.DebtLimitWas(100, Currency.PLN))
            .And(c => c.DebtWas(20, Currency.PLN))
            .When(c => c.AccountIsRecharged(10, Currency.PLN))
            .Then(c => c.AmountAvailableIs(0, Currency.PLN))
            .And(c => c.DebtIs(10, Currency.PLN))
            .Test();

        //The same rule can be passed to multiple scenarios.
        [Fact]
        public void IfDebtCanBeFullyRepaidThenAmountAvailableIsIncreased() => Bdd.Scenario<Context>()
            .Rule(DebtIsAlwaysRepaidInTheFirstPlace)
            .Given(c => c.AmountAvailableWas(0, Currency.PLN))
            .And(c => c.DebtLimitWas(100, Currency.PLN))
            .And(c => c.DebtWas(20, Currency.PLN))
            .When(c => c.AccountIsRecharged(30, Currency.PLN))
            .Then(c => c.AmountAvailableIs(10, Currency.PLN))
            .And(c => c.DebtIs(0, Currency.PLN))
            .Test();

        private class Context
        {
            private Money _amountAvailable;
            private Money _debtLimit;
            private Money _debt;
            private PrePaidAccount _account;

            public void AmountAvailableWas(decimal value, Currency currency) =>
                _amountAvailable = Money.Of(value, currency);

            public void DebtLimitWas(decimal value, Currency currency) => _debtLimit = Money.Of(value, currency);

            public void DebtWas(decimal value, Currency currency) => _debt = Money.Of(value, currency);

            public void NewAccountIsCreated(Currency currency) => _account = PrePaidAccount.New(currency);

            public void AccountIsCharged(decimal value, Currency currency)
            {
                _account = PrePaidAccount.Restore(new PrePaidAccount.Snapshot(_amountAvailable, _debtLimit, _debt));
                _account.Charge(Money.Of(value, currency));
            }

            public void AccountIsRecharged(decimal value, Currency currency)
            {
                _account = PrePaidAccount.Restore(new PrePaidAccount.Snapshot(_amountAvailable, _debtLimit, _debt));
                _account.Recharge(Money.Of(value, currency));
            }

            public void AmountAvailableIs(decimal value, Currency currency) => _account.GetSnapshot()
                .AmountAvailable.Should().Be(Money.Of(value, currency));

            public void DebtIs(decimal value, Currency currency) => _account.GetSnapshot()
                .Debt.Should().Be(Money.Of(value, currency));

            public void PaymentIsRefused(in Result result)
            {
                result.IsSuccessful.Should().BeFalse();
                result.Exception.Should().BeOfType<DomainException>();
            }
        }
    }
}