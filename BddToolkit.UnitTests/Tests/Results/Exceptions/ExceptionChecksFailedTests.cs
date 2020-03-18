using System;
using System.Collections.Immutable;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace ITLIBRIUM.BddToolkit.Tests.Results.Exceptions
{
    public class ExceptionChecksFailedTests
    {
        [Fact]
        public void MessageIsAggregatedForExceptionFromWhenActionAndExceptionChecks()
        {
            var exception1 = GetCaughtException();
            var exception2 = GetCaughtAssertException("Check 1 failed");
            var exception3 = GetCaughtAssertException("Check 2 failed");
            var exceptionChecksFailed = new ExceptionChecksFailed(exception1,
                ImmutableArray.Create(exception2, exception3));
            exceptionChecksFailed.Message.Should().Be(
                $"{Environment.NewLine}" +
                $"Unexpected exception was thrown in When action:{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"{exception1.Message}{Environment.NewLine}" +
                $"{exception1.StackTrace}{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Failed exception checks:{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"1) {exception2.Message}{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"2) {exception3.Message}{Environment.NewLine}");
        }

        [Fact]
        public void ToStringIsAggregatedForExceptionFromWhenActionAndExceptionChecks()
        {
            var exception1 = GetCaughtException();
            var exception2 = GetCaughtAssertException("Check 1 failed");
            var exception3 = GetCaughtAssertException("Check 2 failed");
            var exceptionChecksFailed = new ExceptionChecksFailed(exception1,
                ImmutableArray.Create(exception2, exception3));
            exceptionChecksFailed.ToString().Should().Be(
                $"{Environment.NewLine}" +
                $"Unexpected exception was thrown in When action:{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"{exception1.Message}{Environment.NewLine}" +
                $"{exception1.StackTrace}{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Failed exception checks:{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"1) {exception2.Message}{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"2) {exception3.Message}{Environment.NewLine}");
        }

        [Fact]
        public void OwnStackTraceIsOmitted()
        {
            var exception1 = GetCaughtException();
            var exception2 = GetCaughtAssertException("Check 1 failed");
            var exception3 = GetCaughtAssertException("Check 2 failed");
            var exceptionChecksFailed = new ExceptionChecksFailed(exception1,
                ImmutableArray.Create(exception2, exception3));

            exceptionChecksFailed.StackTrace.Should().Be($"Stack trace of {nameof(ExceptionChecksFailed)} is omitted.");
        }
        
        private static Exception GetCaughtException()
        {
            try
            {
                throw new InvalidOperationException("Custom message");
            }
            catch (Exception e)
            {
                return e;
            }
        }
        
        private static Exception GetCaughtAssertException(string message)
        {
            try
            {
                throw new AssertionFailedException(message);
            }
            catch (Exception e)
            {
                return e;
            }
        }
    }
}