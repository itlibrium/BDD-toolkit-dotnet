using FluentAssertions;
using ITLIBRIUM.BddToolkit.Builders;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.FluentAssertions
{
    public class FluentAssertionsThenBuilder<TContext>
    {
        private readonly IThenContinuationBuilder<TContext> _thenContinuationBuilder;

        protected internal FluentAssertionsThenBuilder(IThenBuilder<TContext> thenBuilder) => 
            _thenContinuationBuilder = thenBuilder.GetContinuationBuilder();

        protected internal FluentAssertionsThenBuilder(IThenContinuationBuilder<TContext> thenContinuationBuilder) => 
            _thenContinuationBuilder = thenContinuationBuilder;

        [PublicAPI]
        public IThenContinuationBuilder<TContext> Throws<TException>() => 
            _thenContinuationBuilder.And((f, e) => e.Should().BeAssignableTo<TException>(), 
                $"Exception type is assignable to {typeof(TException).Name}");

        [PublicAPI]
        public IThenContinuationBuilder<TContext> ThrowsExactly<TException>() => 
            _thenContinuationBuilder.And((f, e) => e.Should().BeOfType<TException>(), 
                $"Exception type is exactly {typeof(TException).Name}");
    }
}