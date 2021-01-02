using System;
using FluentAssertions;
using ITLIBRIUM.BddToolkit.Tests.Results.Exceptions;
using Xunit;

namespace ITLIBRIUM.BddToolkit.FluentAssertions
{
    public class FluentAssertionsThenBuilderTests
    {
        [Fact]
        public void AssertPassWhenExceptionTypeIsEqualToExpected()
        {
            Action test = () => Bdd.Scenario<Context>()
                .When(f => f.BusinessRuleWasBroken())
                .Then().Throws<BusinessException>()
                .Create()
                .RunTest()
                .ThrowOnErrors();
            test.Should().NotThrow();
            
            Action test2 = () => Bdd.Scenario<Context>()
                .When(f => f.BusinessRuleWasBroken())
                .Then().ThrowsExactly<BusinessException>()
                .Create()
                .RunTest()
                .ThrowOnErrors();
            test2.Should().NotThrow();
        }

        [Fact]
        public void AssertPassWhenExceptionTypeIsAssignableToExpected()
        {
            Action test = () => Bdd.Scenario<Context>()
                .When(f => f.BusinessRuleWasBroken())
                .Then().Throws<Exception>()
                .Create()
                .RunTest()
                .ThrowOnErrors();
            test.Should().NotThrow();
        }

        [Fact]
        public void AssertFailWhenExceptionTypeIsNotAssignableToExpected()
        {
            Action test = () => Bdd.Scenario<Context>()
                .When(f => f.BusinessRuleWasBroken())
                .Then().Throws<InvalidOperationException>()
                .Create()
                .RunTest()
                .ThrowOnErrors();
            test.Should().Throw<ExceptionChecksFailed>();
        }

        [Fact]
        public void AssertFailWhenExceptionTypeIsNotEqualToExpected()
        {
            Action test = () => Bdd.Scenario<Context>()
                .When(f => f.BusinessRuleWasBroken())
                .Then().ThrowsExactly<Exception>()
                .Create()
                .RunTest()
                .ThrowOnErrors();
            test.Should().Throw<ExceptionChecksFailed>();
        }
        
        [Fact]
        public void AssertFailWhenExceptionWasNotThrown()
        {
            Action test = () => Bdd.Scenario<Context>()
                .When(f => f.BusinessRuleWasNotBroken())
                .Then().Throws<BusinessException>()
                .Create()
                .RunTest()
                .ThrowOnErrors();
            test.Should().Throw<ExceptionChecksFailed>();
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