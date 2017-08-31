namespace ITLibrium.Bdd.Scenarios
{
    public static class BddScenario
    {
        public static IFixtureBuilder Title(string title)
        {
            return new FixtureBuilder(title);
        }

        public static IGivenBuilder<TFixture> Given<TFixture>()
            where TFixture : class, new()
        {
            return new BddScenarioBuilder<TFixture>(new TFixture());
        }

        public static IGivenBuilder<TFixture> Given<TFixture>(TFixture fixture)
        {
            return new BddScenarioBuilder<TFixture>(fixture);
        }
    }
}