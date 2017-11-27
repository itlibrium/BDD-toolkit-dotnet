using System;
using ITLibrium.Bdd.Scenarios;
using Shouldly;
using Xunit;

namespace ITLibrium.BDD.Tests.Scenarios
{
    public class AggregateAssertExceptionTests
    {
        [Fact]
        public void MessageEqualsSingleExceptionMessage()
        {
            var exception = new Exception("Custom message");
            var aggregateException = new AggregateAssertException(new[]{exception});
            aggregateException.Message.ShouldBe(exception.Message);
        }
        
        [Fact]
        public void ToStringEqualsSingleExceptionToString()
        {
            var exception = new Exception("Custom message");
            var aggregateException = new AggregateAssertException(new[]{exception});
            aggregateException.ToString().ShouldBe(exception.ToString());
        }
        
        [Fact]
        public void MessageIsAggregatedIfMoreThanOneException()
        {
            var exception1 = new Exception("Custom message 1");
            var exception2 = new Exception("Custom message 2");
            var aggregateException = new AggregateAssertException(new[]{exception1, exception2});
            aggregateException.Message.ShouldBe(
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
            aggregateException.ToString().ShouldBe(
                $"More than one assert failed.{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"1) {exception1}{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"2) {exception2}{Environment.NewLine}");
        }
    }
}