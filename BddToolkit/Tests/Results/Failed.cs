using System;
using System.Collections.Immutable;
using System.Linq;
using ITLIBRIUM.BddToolkit.Tests.Results.Exceptions;

namespace ITLIBRIUM.BddToolkit.Tests.Results
{
    internal class Failed : TestResult, IEquatable<Failed>
    {
        private readonly ImmutableArray<Exception> _exceptions;

        protected override bool IsSuccessful => false;

        public Failed(ImmutableArray<Exception> exceptions)
        {
            if (exceptions.IsEmpty)
                throw new ArgumentException($"{nameof(exceptions)} can not be empty");
            _exceptions = exceptions;
        }

        public override void ThrowOnErrors() => throw new AssertsFailed(_exceptions);

        public bool Equals(Failed other) => 
            !(other is null) && 
            _exceptions.SequenceEqual(other._exceptions);

        public override bool Equals(object obj) => obj is Failed other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(_exceptions);
    }
}