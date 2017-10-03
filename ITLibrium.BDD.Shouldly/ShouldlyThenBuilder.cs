using ITLibrium.Bdd.Scenarios;
using Shouldly;

namespace ITLibrium.BDD.Shouldly
{
    public class ShouldlyThenBuilder<TFixture>
    {
        private readonly IThenContinuationBuilder<TFixture> _thenContinuationBuilder;

        public ShouldlyThenBuilder(IThenBuilder<TFixture> thenBuilder)
        {
            _thenContinuationBuilder = thenBuilder.GetContinuationBuilder();
        }

        public ShouldlyThenBuilder(IThenContinuationBuilder<TFixture> thenContinuationBuilder)
        {
            _thenContinuationBuilder = thenContinuationBuilder;
        }

        public IThenContinuationBuilder<TFixture> Throws<TException>()
        {
            return _thenContinuationBuilder.And((f, e) => e.ShouldBeAssignableTo<TException>(), $"Exception type is assignable to {typeof(TException).Name}");
        }

        public IThenContinuationBuilder<TFixture> ThrowsExactly<TException>()
        {
            return _thenContinuationBuilder.And((f, e) => e.ShouldBeOfType<TException>(), $"Exception type is exactly {typeof(TException).Name}");
        }
    }
}