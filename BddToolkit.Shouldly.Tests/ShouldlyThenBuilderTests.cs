using System;
using ITLIBRIUM.BddToolkit.Execution;
using Shouldly;
using Xunit;

namespace ITLIBRIUM.BddToolkit.Shouldly.Tests
{
    public class ShouldlyThenBuilderTests
    {
        [Fact]
        public void AssertPassWhenExceptionTypeIsEqualToExpected()
        {
            Should.NotThrow(() => Bdd.Scenario<Context>()
                .When(f => f.BusinessRuleWasBroken())
                .Then().Throws<BusinessException>()
                .Test());
            
            Should.NotThrow(() => Bdd.Scenario<Context>()
                .When(f => f.BusinessRuleWasBroken())
                .Then().ThrowsExactly<BusinessException>()
                .Test());
        }

        [Fact]
        public void AssertPassWhenExceptionTypeIsAssignableToExpected()
        {
            Should.NotThrow(() => Bdd.Scenario<Context>()
                .When(f => f.BusinessRuleWasBroken())
                .Then().Throws<Exception>()
                .Test());
        }

        [Fact]
        public void AssertFailWhenExceptionTypeIsNotAssignableToExpected()
        {
            Should.Throw<AggregateAssertException>(() => Bdd.Scenario<Context>()
                .When(f => f.BusinessRuleWasBroken())
                .Then().Throws<InvalidOperationException>()
                .Test());
        }

        [Fact]
        public void AssertFailWhenExceptionTypeIsNotEqualToExpected()
        {
            Should.Throw<AggregateAssertException>(() => Bdd.Scenario<Context>()
                .When(f => f.BusinessRuleWasBroken())
                .Then().ThrowsExactly<Exception>()
                .Test());
        }

        private class Context
        {
            public void BusinessRuleWasBroken() => throw new BusinessException();
        }

        private class BusinessException : Exception
        {
        }
    }
}
