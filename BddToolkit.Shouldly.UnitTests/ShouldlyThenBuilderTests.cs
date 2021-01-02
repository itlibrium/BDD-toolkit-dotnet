using System;
using ITLIBRIUM.BddToolkit.Tests.Results.Exceptions;
using Shouldly;
using Xunit;

namespace ITLIBRIUM.BddToolkit.Shouldly
{
    public class ShouldlyThenBuilderTests
    {
        [Fact]
        public void AssertPassWhenExceptionTypeIsEqualToExpected()
        {
            Should.NotThrow(() => Bdd.Scenario<Context>()
                .When(f => f.BusinessRuleWasBroken())
                .Then().Throws<BusinessException>()
                .Create()
                .RunTest()
                .ThrowOnErrors());
            
            Should.NotThrow(() => Bdd.Scenario<Context>()
                .When(f => f.BusinessRuleWasBroken())
                .Then().ThrowsExactly<BusinessException>()
                .Create()
                .RunTest()
                .ThrowOnErrors());
        }

        [Fact]
        public void AssertPassWhenExceptionTypeIsAssignableToExpected()
        {
            Should.NotThrow(() => Bdd.Scenario<Context>()
                .When(f => f.BusinessRuleWasBroken())
                .Then().Throws<Exception>()
                .Create()
                .RunTest()
                .ThrowOnErrors());
        }

        [Fact]
        public void AssertFailWhenExceptionTypeIsNotAssignableToExpected()
        {
            Should.Throw<ExceptionChecksFailed>(() => Bdd.Scenario<Context>()
                .When(f => f.BusinessRuleWasBroken())
                .Then().Throws<InvalidOperationException>()
                .Create()
                .RunTest()
                .ThrowOnErrors());
        }

        [Fact]
        public void AssertFailWhenExceptionTypeIsNotEqualToExpected()
        {
            Should.Throw<ExceptionChecksFailed>(() => Bdd.Scenario<Context>()
                .When(f => f.BusinessRuleWasBroken())
                .Then().ThrowsExactly<Exception>()
                .Create()
                .RunTest()
                .ThrowOnErrors());
        }
        
        [Fact]
        public void AssertFailWhenExceptionWasNotThrown()
        {
            Should.Throw<ExceptionChecksFailed>(() => Bdd.Scenario<Context>()
                .When(f => f.BusinessRuleWasNotBroken())
                .Then().Throws<BusinessException>()
                .Create()
                .RunTest()
                .ThrowOnErrors());
        }

        private class Context
        {
            public void BusinessRuleWasBroken() => throw new BusinessException();
            public void BusinessRuleWasNotBroken() { }
        }

        private class BusinessException : Exception
        {
        }
    }
}
