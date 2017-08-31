namespace ITLibrium.Bdd.Scenarios
{
    public interface IBddScenarioDescription
    {
        string Title { get; }
        string Given { get; }
        string When { get; }
        string Then { get; }
    }
}