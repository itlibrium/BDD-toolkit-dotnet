namespace ITLIBRIUM.BddToolkit.Scenarios
{
    public interface IBddScenarioResult
    {
        IBddScenarioDescription Description { get; }
        bool TestPassed { get; }
    }
}