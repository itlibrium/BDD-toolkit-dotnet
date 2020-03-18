using System;

namespace ITLIBRIUM.BddToolkit.Tests.Results
{
    internal sealed class Passed : TestResult, IEquatable<Passed>
    {
        public static Passed Instance { get; } = new Passed();
        
        protected override bool IsSuccessful => true;

        private Passed() { }

        public override void ThrowOnErrors() { }
        
        public override bool Equals(object obj) => obj is Passed;

        public bool Equals(Passed other) => true;

        public override int GetHashCode() => 0;
    }
}