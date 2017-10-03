using ITLibrium.Bdd.Reports;

namespace ITLibrium.Bdd.Scenarios
{
    public interface IFixtureBuilder
    {
        IFixtureBuilder TestedComponent(string testedComponent);
        
        IFixtureBuilder ReportTo(IBddReport report);
        IFixtureBuilder ExcludeFromReports();
        
        IGivenBuilder<TFixture> Given<TFixture>() where TFixture : class, new();
        IGivenBuilder<TFixture> Given<TFixture>(TFixture fixture);
    }
}