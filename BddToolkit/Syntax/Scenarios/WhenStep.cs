using System;

namespace ITLIBRIUM.BddToolkit.Syntax.Scenarios
{
    public readonly struct WhenStep : ScenarioStep, IEquatable<WhenStep>
    {
        public string Name { get; }

        public WhenStep(string name)
        {
            if(string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            Name = name;
        }

        public bool Equals(WhenStep other) => Name == other.Name;
        public override bool Equals(object obj) => obj is WhenStep other && Equals(other);
        public override int GetHashCode() => Name.GetHashCode();
    }
}