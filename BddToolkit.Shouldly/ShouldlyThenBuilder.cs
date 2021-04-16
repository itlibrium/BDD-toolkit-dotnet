using ITLIBRIUM.BddToolkit.Builders.Scenarios;
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
            _thenContinuationBuilder.And($"Exception type is assignable to {typeof(TException).Name}",
                (_, result) =>
                {
                    result.IsSuccessful.ShouldBeFalse();
                    result.Exception.ShouldBeAssignableTo<TException>();
                });

        [PublicAPI]
        public IThenContinuationBuilder<TContext> ThrowsExactly<TException>() => 
            _thenContinuationBuilder.And($"Exception type is exactly {typeof(TException).Name}", 
                (_, result) =>
                {
                    result.IsSuccessful.ShouldBeFalse();
                    result.Exception.ShouldBeOfType<TException>();
                });
    }
}