using System;
using ITLIBRIUM.BddToolkit.Syntax.Features;

namespace ITLIBRIUM.BddToolkit.Syntax.Rules
{
    public readonly struct Rule : IEquatable<Rule>
    {
        public Feature Feature { get; }
        
        public string Name { get; }
        
        public string Description { get; }

        public Rule(Feature feature, string name, string description)
        {
            Feature = feature;
            Name = name;
            Description = description;
        }

        public bool Equals(Rule other) =>
            (Feature, Name, Description).Equals((other.Feature, other.Name, other.Description));
        public override bool Equals(object obj) => obj is Rule other && Equals(other);
        public override int GetHashCode() => (Feature, Name, Description).GetHashCode();
    }
}