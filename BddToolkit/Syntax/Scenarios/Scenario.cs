using System.Collections.Generic;
using System.Collections.Immutable;
using ITLIBRIUM.BddToolkit.Syntax.Features;
using ITLIBRIUM.BddToolkit.Syntax.Rules;

namespace ITLIBRIUM.BddToolkit.Syntax.Scenarios
{
    public readonly struct Scenario
    {
        public Feature Feature { get; }
        public Rule Rule { get; }
        public string Name { get; }
        public string Description { get; }
        public ImmutableArray<GivenStep> GivenSteps { get; }
        public WhenStep WhenStep { get; }
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