namespace ITLibrium.Bdd.Scenarios
{
    public interface IBddScenarioResult
    {
        IBddScenarioDescription Description { get; }
        bool TestPassed { get; }
    }
}