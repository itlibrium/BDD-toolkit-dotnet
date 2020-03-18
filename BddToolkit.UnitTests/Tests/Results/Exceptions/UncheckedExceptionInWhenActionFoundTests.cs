using System;
using FluentAssertions;
using Xunit;

namespace ITLIBRIUM.BddToolkit.Tests.Results.Exceptions
{
    public class UncheckedExceptionInWhenActionFoundTests
    {
        [Fact]
        public void MessageContainsOriginalMessageAndStackTrace()
        {
            var exception = GetCaughtException();
            var uncheckedExceptionInWhenActionFound = new UncheckedExceptionInWhenActionFound(exception);
            uncheckedExceptionInWhenActionFound.Message.Should().Be(
                $"{Environment.NewLine}" +
                $"Exception was thrown in When action and no exception check was made:{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"{exception.Message}{Environment.NewLine}" +
                $"{exception.StackTrace}{Environment.NewLine}");
        }

        [Fact]
        public void ToStringContainsOriginalMessageAndStackTrace()
        {
            var exception = GetCaughtException();
            var uncheckedExceptionInWhenActionFound = new UncheckedExceptionInWhenActionFound(exception);
            uncheckedExceptionInWhenActionFound.ToString().Should().Be(
                $"{Environment.NewLine}" +
                $"Exception was thrown in When action and no exception check was made:{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"{exception.Message}{Environment.NewLine}" +
                $"{exception.StackTrace}{Environment.NewLine}");
        }

        [Fact]
        public void OwnStackTraceIsOmitted()
        {
            var exception = GetCaughtException();
            var uncheckedExceptionInWhenActionFound = new UncheckedExceptionInWhenActionFound(exception);

            uncheckedExceptionInWhenActionFound.StackTrace.Should()
                .Be($"Stack trace of {nameof(UncheckedExceptionInWhenActionFound)} is omitted.");
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