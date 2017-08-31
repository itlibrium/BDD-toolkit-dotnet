namespace ITLibrium.Bdd.Scenarios
{
    internal class FixtureBuilder : IFixtureBuilder
    {
        private readonly string _title;

        public FixtureBuilder(string title)
        {
            _title = title;
        }

        public IGivenBuilder<TFixture> Given<TFixture>() where TFixture : class, new()
        {
            return new BddScenarioBuilder<TFixture>(new TFixture(), _title);
        }

        public IGivenBuilder<TFixture> Given<TFixture>(TFixture fixture)
        {
            return new BddScenarioBuilder<TFixture>(fixture, _title);
        }
    }
}