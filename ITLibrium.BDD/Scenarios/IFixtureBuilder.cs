using ITLibrium.Bdd.Reports;

namespace ITLibrium.Bdd.Scenarios
{
    public interface IFixtureBuilder
    {
        IFixtureBuilder TestedComponent(string testedComponent);
        
        IFixtureBuilder ReportTo(IBddReport report);
        IFixtureBuilder ExcludeFromReports();
        
        IGivenContinuationBuilder<TFixture> Given<TFixture>() where TFixture : class, new();
        IGivenContinuationBuilder<TFixture> Given<TFixture>(TFixture fixture);
    }
}