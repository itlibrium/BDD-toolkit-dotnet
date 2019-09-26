using ITLIBRIUM.BddToolkit.Builders;
using ITLIBRIUM.BddToolkit.Configuration;
using ITLIBRIUM.BddToolkit.Syntax.Features;
using ITLIBRIUM.BddToolkit.Syntax.Rules;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit
{
    public static class Bdd
    {
        private static readonly ConfigValues _configValues = ConfigValues.Default();
        public static ConfigValues Setup() => _configValues;
        
        [PublicAPI]
        public static Feature Feature(string name) => new Feature(name, default);
        
        [PublicAPI]
        public static Rule Rule(Feature feature, string name) => new Rule(feature, name, default);
        
        [PublicAPI]
        public static IScenarioDescriptionBuilder<TContext> Scenario<TContext>() where TContext : new() => 
            new ScenarioBuilder<TContext>(new TContext(), _configValues.DocPublisher);
        
        [PublicAPI]
        public static IScenarioDescriptionBuilder<TContext> Scenario<TContext>(TContext context) => 
            new ScenarioBuilder<TContext>(context, _configValues.DocPublisher);
    }
}