using ITLIBRIUM.BddToolkit.Builders;
using JetBrains.Annotations;
using Shouldly;

namespace ITLIBRIUM.BddToolkit.Shouldly
{
    public class ShouldlyThenBuilder<TContext>
    {
        private readonly IThenContinuationBuilder<TContext> _thenContinuationBuilder;

        protected internal ShouldlyThenBuilder(IThenBuilder<TContext> thenBuilder) => 
            _thenContinuationBuilder = thenBuilder.GetContinuationBuilder();

        protected internal ShouldlyThenBuilder(IThenContinuationBuilder<TContext> thenContinuationBuilder) => 
            _thenContinuationBuilder = thenContinuationBuilder;

        [PublicAPI]
        public IThenContinuationBuilder<TContext> Throws<TException>() => 
            _thenContinuationBuilder.And((f, e) => e.ShouldBeAssignableTo<TException>(), 
                $"Exception type is assignable to {typeof(TException).Name}");

        [PublicAPI]
        public IThenContinuationBuilder<TContext> ThrowsExactly<TException>() => 
            _thenContinuationBuilder.And((f, e) => e.ShouldBeOfType<TException>(), 
                $"Exception type is exactly {typeof(TException).Name}");
    }
}