using System;
using System.Collections.Immutable;
using FluentAssertions;
using Xunit;

namespace ITLIBRIUM.BddToolkit.Execution
{
    public class AggregateAssertExceptionTests
    {
        [Fact]
        public void MessageEqualsSingleExceptionMessage()
        {
            var exception = new Exception("Custom message");
            var aggregateException = new AggregateAssertException(ImmutableArray.Create(exception));
            aggregateException.Message.Should().Be(exception.Message);
        }
        
        [Fact]
        public void ToStringEqualsSingleExceptionToString()
        {
            var exception = new Exception("Custom message");
            var aggregateException = new AggregateAssertException(ImmutableArray.Create(exception));
            aggregateException.ToString().Should().Be(exception.ToString());
        }
        
        [Fact]
        public void MessageIsAggregatedForMoreThanOneException()
        {
            var exception1 = new Exception("Custom message 1");
            var exception2 = new Exception("Custom message 2");
            var aggregateException = new AggregateAssertException(ImmutableArray.Create(exception1, exception2));
            aggregateException.Message.Should().Be(
                $"More than one assert failed.{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"1) {exception1.Message}{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"2) {exception2.Message}{Environment.NewLine}");
        }
        
        [Fact]
        public void ToStringIsAggregatedForMoreThanOneException()
        {
            var exception1 = new Exception("Custom message 1");
            var exception2 = new Exception("Custom message 2");
            var aggregateException = new AggregateAssertException(ImmutableArray.Create(exception1, exception2));
            aggregateException.ToString().Should().Be(
                $"More than one assert failed.{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"1) {exception1}{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"2) {exception2}{Environment.NewLine}");
        }
    }
}