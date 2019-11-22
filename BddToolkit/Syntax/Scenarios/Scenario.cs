using System.Collections.Generic;
using System.Collections.Immutable;
using ITLIBRIUM.BddToolkit.Syntax.Features;
using ITLIBRIUM.BddToolkit.Syntax.Rules;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Syntax.Scenarios
{
    public readonly struct Scenario
    {
        [PublicAPI]
        public Feature Feature { get; }
        
        [PublicAPI]
        public Rule Rule { get; }
        
        [PublicAPI]
        public string Name { get; }
        
        [PublicAPI]
        public string Description { get; }
        
        [PublicAPI]
        public ImmutableArray<GivenStep> GivenSteps { get; }
        
        [PublicAPI]
        public WhenStep WhenStep { get; }
        
        [PublicAPI]
        public ImmutableArray<ThenStep> ThenSteps { get; }

        public Scenario(Feature feature, Rule rule, string name, string description, IEnumerable<GivenStep> givenSteps, 
            WhenStep whenStep, IEnumerable<ThenStep> thenSteps)
        {
            Feature = feature;
            Rule = rule;
            Name = name;
            Description = description;
            GivenSteps = givenSteps.ToImmutableArray();
            WhenStep = whenStep;
            ThenSteps = thenSteps.ToImmutableArray();
        }
    }
}