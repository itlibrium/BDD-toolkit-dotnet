using System;

namespace ITLIBRIUM.BddToolkit.Syntax.Features
{
    public readonly struct Feature : IEquatable<Feature>
    {
        public string Name { get; }
        public string Description { get; }

        public Feature(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public bool Equals(Feature other) => (Name, Description).Equals((other.Name, other.Description));
        public override bool Equals(object obj) => obj is Feature other && Equals(other);
        public override int GetHashCode() => (Name, Description).GetHashCode();
    }
}