using System;
using System.Collections.Immutable;
using System.Linq;
using ITLIBRIUM.BddToolkit.Tests.Results.Exceptions;

namespace ITLIBRIUM.BddToolkit.Tests.Results
{
    internal class NoExpectedExceptionInWhenAction : TestResult, IEquatable<NoExpectedExceptionInWhenAction>
    {
        private readonly ImmutableArray<Exception> _failedExceptionChecks;

        protected override bool IsSuccessful => false;

        public NoExpectedExceptionInWhenAction(ImmutableArray<Exception> failedExceptionChecks)
        {
            if (failedExceptionChecks.IsEmpty)
                throw new ArgumentException($"{nameof(failedExceptionChecks)} can not be empty");
            _failedExceptionChecks = failedExceptionChecks;
        }

        public override void ThrowOnErrors() => throw new ExceptionChecksFailed(_failedExceptionChecks);

        public bool Equals(NoExpectedExceptionInWhenAction other) =>
            !(other is null) &&
            _failedExceptionChecks.SequenceEqual(other._failedExceptionChecks);

        public override bool Equals(object obj) => obj is NoExpectedExceptionInWhenAction other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(_failedExceptionChecks);
    }
}