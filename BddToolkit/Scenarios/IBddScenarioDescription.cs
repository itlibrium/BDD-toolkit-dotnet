namespace ITLIBRIUM.BddToolkit.Scenarios
{
    public interface IBddScenarioDescription
    {
        string TestedComponent { get; }
        string Title { get; }
        string Given { get; }
        string When { get; }
        string Then { get; }
    }
}