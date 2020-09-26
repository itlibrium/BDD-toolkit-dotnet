using System;

namespace ITLIBRIUM.BddToolkit.Examples
{
    public readonly struct PrePaidAccountId : IEquatable<PrePaidAccountId>
    {
        public Guid Value { get; }

        public static PrePaidAccountId New() => new PrePaidAccountId(Guid.NewGuid());
        
        public static PrePaidAccountId Of(Guid value) => new PrePaidAccountId(value);
        
        private PrePaidAccountId(Guid value) => Value = value;

        public bool Equals(PrePaidAccountId other) => Value.Equals(other.Value);

        public override bool Equals(object obj) => obj is PrePaidAccountId other && Equals(other);

        public override int GetHashCode() => Value.GetHashCode();
    }
}