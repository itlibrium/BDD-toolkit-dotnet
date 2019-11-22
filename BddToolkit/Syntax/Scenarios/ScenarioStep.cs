using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Syntax.Scenarios
{
    internal interface ScenarioStep
    {
        [PublicAPI]
        string Name { get; }
    }
}