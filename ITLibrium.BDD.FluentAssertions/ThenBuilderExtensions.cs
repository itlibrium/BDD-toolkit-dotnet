using ITLibrium.Bdd.Scenarios;

namespace ITLibrium.BDD.FluentAssertions
{
    public static class ThenBuilderExtensions
    {
        public static FluentAssertionsThenBuilder<TContext> Then<TContext>(this IThenBuilder<TContext> thenBuilder)
        {
            return new FluentAssertionsThenBuilder<TContext>(thenBuilder);
        }

        public static FluentAssertionsThenBuilder<TContext> And<TContext>(this IThenContinuationBuilder<TContext> thenBuilder)
        {
            return new FluentAssertionsThenBuilder<TContext>(thenBuilder);
        }
    }
}