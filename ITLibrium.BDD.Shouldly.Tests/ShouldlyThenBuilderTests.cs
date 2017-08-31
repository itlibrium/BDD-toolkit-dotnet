using System;
using ITLibrium.Bdd.Scenarios;
using Shouldly;
using Xunit;

namespace ITLibrium.BDD.Shouldly.Tests
{
    public class ShouldlyThenBuilderTests
    {
        [Fact]
        public void ThrowsAssertPassWhenExceptionTypeIsEqualToExpected()
        {
            Should.NotThrow(() => BddScenario
                .Given<Fixture>()
                .GivenNoAction()
                .When(f => f.BusinessRuleWasBroken())
                .Then().Throws<BusinessException>()
                .Test());
        }

        [Fact]
        public void ThrowsAssertPassWhenExceptionTypeIsAssignableToExpected()
        {
            Should.NotThrow(() => BddScenario
                .Given<Fixture>()
                .GivenNoAction()
                .When(f => f.BusinessRuleWasBroken())
                .Then().Throws<Exception>()
                .Test());
        }

        [Fact]
        public void ThrowsAssertFailWhenExceptionTypeIsNotAssignableToExpected()
        {
            Should.Throw<AggregateAssertException>(() => BddScenario
                .Given<Fixture>()
                .GivenNoAction()
                .When(f => f.BusinessRuleWasBroken())
                .Then().Throws<InvalidOperationException>()
                .Test());
        }

        [Fact]
        public void ThrowsExactlyAssertPassWhenExceptionTypeIsEqualToExpected()
        {
            Should.NotThrow(() => BddScenario
                .Given<Fixture>()
                .GivenNoAction()
                .When(f => f.BusinessRuleWasBroken())
                .Then().ThrowsExactly<BusinessException>()
                .Test());
        }

        [Fact]
        public void ThrowsExactlyAssertFailWhenExceptionTypeIsNotEqualToExpected()
        {
            Should.Throw<AggregateAssertException>(() => BddScenario
                .Given<Fixture>()
                .GivenNoAction()
                .When(f => f.BusinessRuleWasBroken())
                .Then().ThrowsExactly<Exception>()
                .Test());
        }

        private class Fixture
        {
            public void BusinessRuleWasBroken()
            {
                throw new BusinessException();
            }
        }

        private class BusinessException : Exception
        {
            
        }
    }
}
