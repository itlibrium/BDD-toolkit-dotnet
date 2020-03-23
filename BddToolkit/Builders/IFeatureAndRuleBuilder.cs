using ITLIBRIUM.BddToolkit.Syntax.Features;
using ITLIBRIUM.BddToolkit.Syntax.Rules;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Builders
{
    public interface IFeatureAndRuleBuilder<TContext> : INameBuilder<TContext>
    {
        [PublicAPI]
        INameBuilder<TContext> Feature(in Feature feature);
        
        [PublicAPI]
        INameBuilder<TContext> Rule(in Rule rule);
    }
}