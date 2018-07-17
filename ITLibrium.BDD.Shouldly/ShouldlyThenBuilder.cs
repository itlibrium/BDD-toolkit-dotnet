using ITLibrium.Bdd.Scenarios;
using Shouldly;

namespace ITLibrium.BDD.Shouldly
{
    public class ShouldlyThenBuilder<TContext>
    {
        private readonly IThenContinuationBuilder<TContext> _thenContinuationBuilder;

        public ShouldlyThenBuilder(IThenBuilder<TContext> thenBuilder)
        {
            _thenContinuationBuilder = thenBuilder.GetContinuationBuilder();
        }

        public ShouldlyThenBuilder(IThenContinuationBuilder<TContext> thenContinuationBuilder)
        {
            _thenContinuationBuilder = thenContinuationBuilder;
        }

        public IThenContinuationBuilder<TContext> Throws<TException>()
        {
            return _thenContinuationBuilder.And((f, e) => e.ShouldBeAssignableTo<TException>(), $"Exception type is assignable to {typeof(TException).Name}");
        }

        public IThenContinuationBuilder<TContext> ThrowsExactly<TException>()
        {
            return _thenContinuationBuilder.And((f, e) => e.ShouldBeOfType<TException>(), $"Exception type is exactly {typeof(TException).Name}");
        }
    }
}