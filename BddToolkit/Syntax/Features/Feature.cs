using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Syntax.Features
{
    public readonly struct Feature : IEquatable<Feature>
    {
        [PublicAPI]
        public string Name { get; }

        [PublicAPI]
        public string Description { get; }
        
        [PublicAPI]
        public ImmutableArray<string> Tags { get; }

        internal bool IsEmpty => string.IsNullOrWhiteSpace(Name);

        public static Feature Empty() => new Feature(default, default, Enumerable.Empty<string>());

        public static Feature New([NotNull] string name, string description, IEnumerable<string> tags)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            return new Feature(name, description, tags);
        }

        private Feature(string name, string description, IEnumerable<string> tags)
        {
            Name = name;
            Description = description;
            Tags = tags.ToImmutableArray();
        }

        public bool Equals(Feature other) => Name == other.Name;
        public override bool Equals(object obj) => obj is Feature other && Equals(other);
        public override int GetHashCode() => Name is null ? 0 : Name.GetHashCode();
    }
}