using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ITLIBRIUM.BddToolkit.Syntax.Features;
using ITLIBRIUM.BddToolkit.Syntax.Rules;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Syntax.Scenarios
{
    public readonly struct Scenario : IEquatable<Scenario>
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
        public ImmutableArray<string> Tags { get; }
        
        [PublicAPI]
        public ImmutableArray<GivenStep> GivenSteps { get; }
        
        [PublicAPI]
        public WhenStep WhenStep { get; }
        
        [PublicAPI]
        public ImmutableArray<ThenStep> ThenSteps { get; }

        public Scenario(in Feature feature, in Rule rule, string name, string description, IEnumerable<string> tags, 
            IEnumerable<GivenStep> givenSteps, WhenStep whenStep, IEnumerable<ThenStep> thenSteps)
        {
            if (!rule.IsEmpty && !feature.Equals(rule.Feature))
                throw new ArgumentException(
                    $"{nameof(Rule)} is assigned to different {nameof(Feature)} then {nameof(Scenario)}");
            Feature = feature;
            Rule = rule;
            Name = name;
            Description = description;
            Tags = tags.ToImmutableArray();
            GivenSteps = givenSteps.ToImmutableArray();
            WhenStep = whenStep;
            ThenSteps = thenSteps.ToImmutableArray();
        }

        public bool Equals(Scenario other) =>
            Feature.Equals(other.Feature) && 
            Rule.Equals(other.Rule) && 
            Name == other.Name && 
            Description == other.Description && 
            GivenSteps.SequenceEqual(other.GivenSteps) &&
            WhenStep.Equals(other.WhenStep) &&
            ThenSteps.SequenceEqual(other.ThenSteps);

        public override bool Equals(object obj) => obj is Scenario other && Equals(other);

        public override int GetHashCode() =>
            (Feature, Name, Description, HashCode.Combine(GivenSteps), WhenStep, HashCode.Combine(ThenSteps))
            .GetHashCode();
    }
}