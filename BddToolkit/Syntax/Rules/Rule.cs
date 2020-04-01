using System;
using ITLIBRIUM.BddToolkit.Syntax.Features;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Syntax.Rules
{
    public readonly struct Rule : IEquatable<Rule>
    {
        [PublicAPI]
        public Feature Feature { get; }

        [PublicAPI]
        public string Name { get; }

        [PublicAPI]
        public string Description { get; }

        internal bool IsEmpty => string.IsNullOrWhiteSpace(Name);

        public static Rule Empty() => new Rule(Feature.Empty(), default, default);

        public static Rule New(Feature feature, [NotNull] string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            return new Rule(feature, name, description);
        }

        private Rule(in Feature feature, string name, string description)
        {
            Feature = feature;
            Name = name;
            Description = description;
        }

        public bool Equals(Rule other) => (Feature, Name).Equals((other.Feature, other.Name));
        public override bool Equals(object obj) => obj is Rule other && Equals(other);
        public override int GetHashCode() => (Feature, Name).GetHashCode();
    }
}