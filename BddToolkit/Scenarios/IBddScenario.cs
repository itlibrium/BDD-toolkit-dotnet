namespace ITLIBRIUM.BddToolkit.Scenarios
{
    public interface IBddScenario
    {
        IBddScenarioDescription GetDescription();
        void Test();
    }
}