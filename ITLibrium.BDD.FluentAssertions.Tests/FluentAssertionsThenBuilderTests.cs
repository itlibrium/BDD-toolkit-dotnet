using System;
using FluentAssertions;
using ITLibrium.Bdd.Scenarios;
using Xunit;

namespace ITLibrium.BDD.FluentAssertions.Tests
{
    public class FluentAssertionsThenBuilderTests
    {
        [Fact]
        public void AssertPassWhenExceptionTypeIsEqualToExpected()
        {
            Action test = () => BddScenario
                .ExcludeFromReports()
                .Using<Fixture>()
                .When(f => f.BusinessRuleWasBroken())
                .Then().Throws<BusinessException>()
                .Test();
            test.Should().NotThrow();
            
            Action test2 = () => BddScenario
                .ExcludeFromReports()
                .Using<Fixture>()
                .When(f => f.BusinessRuleWasBroken())
                .Then().ThrowsExactly<BusinessException>()
                .Test();
            test2.Should().NotThrow();
        }

        [Fact]
        public void AssertPassWhenExceptionTypeIsAssignableToExpected()
        {
            Action test = () => BddScenario
                .ExcludeFromReports()
                .Using<Fixture>()
                .When(f => f.BusinessRuleWasBroken())
                .Then().Throws<Exception>()
                .Test();
            test.Should().NotThrow();
        }

        [Fact]
        public void AssertFailWhenExceptionTypeIsNotAssignableToExpected()
        {
            Action test = () => BddScenario
                .ExcludeFromReports()
                .Using<Fixture>()
                .When(f => f.BusinessRuleWasBroken())
                .Then().Throws<InvalidOperationException>()
                .Test();
            test.Should().Throw<AggregateAssertException>();
        }

        [Fact]
        public void AssertFailWhenExceptionTypeIsNotEqualToExpected()
        {
            Action test = () => BddScenario
                .ExcludeFromReports()
                .Using<Fixture>()
                .When(f => f.BusinessRuleWasBroken())
                .Then().ThrowsExactly<Exception>()
                .Test();
            test.Should().Throw<AggregateAssertException>();
        }

        private class Fixture
        {
            public void BusinessRuleWasBroken() => throw new BusinessException();
        }

        private class BusinessException : Exception
        {
        }
    }
}