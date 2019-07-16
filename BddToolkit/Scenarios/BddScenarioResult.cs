namespace ITLIBRIUM.BddToolkit.Scenarios
{
    internal class BddScenarioResult : IBddScenarioResult
    {
        public IBddScenarioDescription Description { get; }
        public bool TestPassed { get; }

        public BddScenarioResult(IBddScenarioDescription description, bool testPassed)
        {
            Description = description;
            TestPassed = testPassed;
        }
    }
}