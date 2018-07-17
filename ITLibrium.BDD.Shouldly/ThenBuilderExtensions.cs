using ITLibrium.Bdd.Scenarios;

namespace ITLibrium.BDD.Shouldly
{
    public static class ThenBuilderExtensions
    {
        public static ShouldlyThenBuilder<TContext> Then<TContext>(this IThenBuilder<TContext> thenBuilder)
        {
            return new ShouldlyThenBuilder<TContext>(thenBuilder);
        }

        public static ShouldlyThenBuilder<TContext> And<TContext>(this IThenContinuationBuilder<TContext> thenBuilder)
        {
            return new ShouldlyThenBuilder<TContext>(thenBuilder);
        }
    }
}