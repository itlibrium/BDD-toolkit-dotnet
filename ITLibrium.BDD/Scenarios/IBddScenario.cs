namespace ITLibrium.Bdd.Scenarios
{
    public interface IBddScenario
    {
        IBddScenarioDescription GetDescription();
        void Test();
    }
}