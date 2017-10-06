using ITLibrium.Bdd.Reports;

namespace ITLibrium.Bdd.Scenarios
{
    public static class BddScenario
    {
        public static IFixtureBuilder TestedComponent(string testedComponent)
        {
            return new FixtureBuilder(testedComponent, null, false, null);
        }

        public static IFixtureBuilder Title(string title)
        {
            return new FixtureBuilder(null, title, false, null);
        }

        public static IFixtureBuilder ReportTo(IBddReport report)
        {
            return new FixtureBuilder(null, null, false, report);
        }
        
        public static IFixtureBuilder ExcludeFromReports()
        {
            return new FixtureBuilder(null, null, true, null);
        }

        public static IGivenContinuationBuilder<TFixture> Given<TFixture>()
            where TFixture : class, new()
        {
            return new BddScenarioBuilder<TFixture>(null, null, new TFixture(), false,  null);
        }

        public static IGivenContinuationBuilder<TFixture> Given<TFixture>(TFixture fixture)
        {
            return new BddScenarioBuilder<TFixture>(null, null, fixture, false, null);
        }
    }
}