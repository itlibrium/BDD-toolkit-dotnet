using ITLibrium.Bdd.Scenarios;
using JetBrains.Annotations;

namespace ITLibrium.BDD.FluentAssertions
{
    public static class ThenBuilderExtensions
    {
        [PublicAPI]
        public static FluentAssertionsThenBuilder<TContext> Then<TContext>(
            this IThenBuilder<TContext> thenBuilder) => 
            new FluentAssertionsThenBuilder<TContext>(thenBuilder);

        [PublicAPI]
        public static FluentAssertionsThenBuilder<TContext> And<TContext>(
            this IThenContinuationBuilder<TContext> thenBuilder) => 
            new FluentAssertionsThenBuilder<TContext>(thenBuilder);
    }
}