using FluentAssertions;
using ITLibrium.Bdd.Scenarios;

namespace ITLibrium.BDD.FluentAssertions
{
    public class FluentAssertionsThenBuilder<TContext>
    {
        private readonly IThenContinuationBuilder<TContext> _thenContinuationBuilder;

        public FluentAssertionsThenBuilder(IThenBuilder<TContext> thenBuilder)
        {
            _thenContinuationBuilder = thenBuilder.GetContinuationBuilder();
        }

        public FluentAssertionsThenBuilder(IThenContinuationBuilder<TContext> thenContinuationBuilder)
        {
            _thenContinuationBuilder = thenContinuationBuilder;
        }

        public IThenContinuationBuilder<TContext> Throws<TException>()
        {
            return _thenContinuationBuilder.And((f, e) => e.Should().BeAssignableTo<TException>(), $"Exception type is assignable to {typeof(TException).Name}");
        }

        public IThenContinuationBuilder<TContext> ThrowsExactly<TException>()
        {
            return _thenContinuationBuilder.And((f, e) => e.Should().BeOfType<TException>(), $"Exception type is exactly {typeof(TException).Name}");
        }
    }
}