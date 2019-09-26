using System;

namespace ITLIBRIUM.BddToolkit.Syntax.Scenarios
{
    public readonly struct GivenStep : ScenarioStep, IEquatable<GivenStep>
    {
        public string Name { get; }

        public GivenStep(string name)
        {
            if(string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            Name = name;
        }

        public bool Equals(GivenStep other) => Name == other.Name;
        public override bool Equals(object obj) => obj is GivenStep other && Equals(other);
        public override int GetHashCode() => Name.GetHashCode();
    }
}