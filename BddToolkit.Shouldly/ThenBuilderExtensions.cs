using ITLIBRIUM.BddToolkit.Scenarios;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Shouldly
{
    public static class ThenBuilderExtensions
    {
        [PublicAPI]
        public static ShouldlyThenBuilder<TContext> Then<TContext>(
            this IThenBuilder<TContext> thenBuilder) => 
            new ShouldlyThenBuilder<TContext>(thenBuilder);

        [PublicAPI]
        public static ShouldlyThenBuilder<TContext> And<TContext>(
            this IThenContinuationBuilder<TContext> thenBuilder) => 
            new ShouldlyThenBuilder<TContext>(thenBuilder);
    }
}