using System;
using ITLIBRIUM.BddToolkit.Reports;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Scenarios
{
    public static class BddScenario
    {
        [PublicAPI]
        public static IScenarioMetadataBuilder TestedComponent(string testedComponent) => 
            new ScenarioMetadataBuilder(testedComponent, null, false, null);

        [PublicAPI]
        public static IScenarioMetadataBuilder Title(string title) => 
            new ScenarioMetadataBuilder(null, title, false, null);

        [PublicAPI]
        public static IScenarioMetadataBuilder ReportTo(IBddReport report) => 
            new ScenarioMetadataBuilder(null, null, false, report);

        [PublicAPI]
        public static IScenarioMetadataBuilder ExcludeFromReports() => 
            new ScenarioMetadataBuilder(null, null, true, null);

        [PublicAPI]
        public static IGivenBuilder<TContext> Using<TContext>() where TContext : class, new() =>
            new BddScenarioBuilder<TContext>(null, null, new TContext(), false,  null);

        [PublicAPI]
        public static IGivenBuilder<TContext> Using<TContext>(TContext fixture) => 
            new BddScenarioBuilder<TContext>(null, null, fixture, false, null);
    }
}