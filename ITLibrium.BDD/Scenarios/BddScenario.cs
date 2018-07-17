using System;
using ITLibrium.Bdd.Reports;

namespace ITLibrium.Bdd.Scenarios
{
    public static class BddScenario
    {
        public static IScenarioMetadataBuilder TestedComponent(string testedComponent)
        {
            return new ScenarioMetadataBuilder(testedComponent, null, false, null);
        }

        public static IScenarioMetadataBuilder Title(string title)
        {
            return new ScenarioMetadataBuilder(null, title, false, null);
        }

        public static IScenarioMetadataBuilder ReportTo(IBddReport report)
        {
            return new ScenarioMetadataBuilder(null, null, false, report);
        }
        
        public static IScenarioMetadataBuilder ExcludeFromReports()
        {
            return new ScenarioMetadataBuilder(null, null, true, null);
        }
        
        public static IGivenBuilder<TContext> Using<TContext>()
            where TContext : class, new()
        {
            return new BddScenarioBuilder<TContext>(null, null, new TContext(), false,  null);
        }

        public static IGivenBuilder<TContext> Using<TContext>(TContext fixture)
        {
            return new BddScenarioBuilder<TContext>(null, null, fixture, false, null);
        }

        [Obsolete("Use 'Using' method instead")]
        public static IGivenContinuationBuilder<TContext> Given<TContext>()
            where TContext : class, new()
        {
            return new BddScenarioBuilder<TContext>(null, null, new TContext(), false,  null);
        }

        [Obsolete("Use 'Using' method instead")]
        public static IGivenContinuationBuilder<TContext> Given<TContext>(TContext fixture)
        {
            return new BddScenarioBuilder<TContext>(null, null, fixture, false, null);
        }
    }
}