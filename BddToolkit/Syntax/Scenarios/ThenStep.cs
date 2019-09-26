using System;

namespace ITLIBRIUM.BddToolkit.Syntax.Scenarios
{
    public readonly struct ThenStep : ScenarioStep, IEquatable<ThenStep>
    {
        public string Name { get; }

        public ThenStep(string name)
        {
            if(string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            Name = name;
        }

        public bool Equals(ThenStep other) => Name == other.Name;
        public override bool Equals(object obj) => obj is ThenStep other && Equals(other);
        public override int GetHashCode() => Name.GetHashCode();
    }
}