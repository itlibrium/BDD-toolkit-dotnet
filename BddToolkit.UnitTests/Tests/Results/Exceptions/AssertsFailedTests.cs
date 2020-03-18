using System;
using System.Collections.Immutable;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace ITLIBRIUM.BddToolkit.Tests.Results.Exceptions
{
    public class AssertsFailedTests
    {
        [Fact]
        public void MessageIsAggregatedForAllExceptions()
        {
            var exception1 = GetCaughtAssertException("Assert 1 failed");
            var exception2 = GetCaughtAssertException("Assert 2 failed");
            var assertsFailed = new AssertsFailed(ImmutableArray.Create(exception1, exception2));
            assertsFailed.Message.Should().Be(
                $"{Environment.NewLine}" +
                $"One or more assert failed:{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"1) {exception1.Message}{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"2) {exception2.Message}{Environment.NewLine}");
        }
        
        [Fact]
        public void ToStringIsAggregatedForAllExceptions()
        {
            var exception1 = GetCaughtAssertException("Assert 1 failed");
            var exception2 = GetCaughtAssertException("Assert 2 failed");
            var assertsFailed = new AssertsFailed(ImmutableArray.Create(exception1, exception2));
            assertsFailed.ToString().Should().Be(
                $"{Environment.NewLine}" +
                $"One or more assert failed:{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"1) {exception1.Message}{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"2) {exception2.Message}{Environment.NewLine}");
        }
        
        [Fact]
        public void OwnStackTraceIsOmitted()
        {
            var exception1 = GetCaughtAssertException("Assert 1 failed");
            var exception2 = GetCaughtAssertException("Assert 2 failed");
            var assertsFailed = new AssertsFailed(ImmutableArray.Create(exception1, exception2));

            assertsFailed.StackTrace.Should().Be($"Stack trace of {nameof(AssertsFailed)} is omitted.");
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