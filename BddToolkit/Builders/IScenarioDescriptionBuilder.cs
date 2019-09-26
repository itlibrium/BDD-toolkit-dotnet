using ITLIBRIUM.BddToolkit.Syntax.Features;
using ITLIBRIUM.BddToolkit.Syntax.Rules;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Builders
{
    public interface IScenarioDescriptionBuilder<TContext> : IGivenBuilder<TContext>
    {
        [PublicAPI]
        IScenarioDescriptionBuilder<TContext> Feature(Feature feature);
        
        [PublicAPI]
        IScenarioDescriptionBuilder<TContext> Rule(Rule rule);
        
        [PublicAPI]
        IScenarioDescriptionBuilder<TContext> Name(string name);
        
        [PublicAPI]
        IScenarioDescriptionBuilder<TContext> Description(string description);
    }
}