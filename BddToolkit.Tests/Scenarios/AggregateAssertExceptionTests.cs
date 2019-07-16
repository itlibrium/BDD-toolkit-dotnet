using System;
using FluentAssertions;
using ITLIBRIUM.BddToolkit.Scenarios;
using Xunit;

namespace ITLIBRIUM.BddToolkit.Tests.Scenarios
{
    public class AggregateAssertExceptionTests
    {
        [Fact]
        public void MessageEqualsSingleExceptionMessage()
        {
            var exception = new Exception("Custom message");
            var aggregateException = new AggregateAssertException(new[]{exception});
            aggregateException.Message.Should().Be(exception.Message);
        }
        
        [Fact]
        public void ToStringEqualsSingleExceptionToString()
        {
            var exception = new Exception("Custom message");
            var aggregateException = new AggregateAssertException(new[]{exception});
            aggregateException.ToString().Should().Be(exception.ToString());
        }
        
        [Fact]
        public void MessageIsAggregatedIfMoreThanOneException()
        {
            var exception1 = new Exception("Custom message 1");
            var exception2 = new Exception("Custom message 2");
            var aggregateException = new AggregateAssertException(new[]{exception1, exception2});
            aggregateException.Message.Should().Be(
                $"More than one assert failed.{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"1) {exception1.Message}{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"2) {exception2.Message}{Environment.NewLine}");
        }
        
        [Fact]
        public void ToStringIsAggregatedIfMoreThanOneException()
        {
            var exception1 = new Exception("Custom message 1");
            var exception2 = new Exception("Custom message 2");
            var aggregateException = new AggregateAssertException(new[]{exception1, exception2});
            aggregateException.ToString().Should().Be(
                $"More than one assert failed.{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"1) {exception1}{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"2) {exception2}{Environment.NewLine}");
        }
    }
}