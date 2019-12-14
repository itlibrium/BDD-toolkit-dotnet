using System;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Syntax.Features
{
    public readonly struct Feature : IEquatable<Feature>
    {
        [PublicAPI]
        public string Name { get; }
        
        [PublicAPI]
        public string Description { get; }

        public Feature([NotNull] string name, string description = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
        }

        public bool Equals(Feature other) => Name == other.Name;
        public override bool Equals(object obj) => obj is Feature other && Equals(other);
        public override int GetHashCode() => Name.GetHashCode();
    }
}