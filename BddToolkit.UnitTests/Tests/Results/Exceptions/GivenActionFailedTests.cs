using System;
using FluentAssertions;
using Xunit;

namespace ITLIBRIUM.BddToolkit.Tests.Results.Exceptions
{
    public class GivenActionFailedTests
    {
        [Fact]
        public void MessageContainsOriginalMessageAndStackTrace()
        {
            var exception = GetCaughtException();
            var givenActionFailed = new GivenActionFailed(exception);
            givenActionFailed.Message.Should().Be(
                $"{Environment.NewLine}" +
                $"Exception was thrown in Given action:{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"{exception.Message}{Environment.NewLine}" +
                $"{exception.StackTrace}{Environment.NewLine}");
        }

        [Fact]
        public void ToStringContainsOriginalMessageAndStackTrace()
        {
            var exception = GetCaughtException();
            var givenActionFailed = new GivenActionFailed(exception);
            givenActionFailed.ToString().Should().Be(
                $"{Environment.NewLine}" +
                $"Exception was thrown in Given action:{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"{exception.Message}{Environment.NewLine}" +
                $"{exception.StackTrace}{Environment.NewLine}");
        }

        [Fact]
        public void OwnStackTraceIsOmitted()
        {
            var exception = GetCaughtException();
            var givenActionFailed = new GivenActionFailed(exception);

            givenActionFailed.StackTrace.Should().Be($"Stack trace of {nameof(GivenActionFailed)} is omitted.");
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
    }
}