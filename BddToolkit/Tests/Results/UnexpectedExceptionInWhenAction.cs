using System;
using System.Collections.Immutable;
using System.Linq;
using ITLIBRIUM.BddToolkit.Tests.Results.Exceptions;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Tests.Results
{
    internal class UnexpectedExceptionInWhenAction : TestResult, IEquatable<UnexpectedExceptionInWhenAction>
    {
        private readonly Exception _exceptionFromWhenAction;
        private readonly ImmutableArray<Exception> _failedExceptionChecks;

        protected override bool IsSuccessful => false;

        public UnexpectedExceptionInWhenAction([NotNull] Exception exceptionFromWhenAction,
            ImmutableArray<Exception> failedExceptionChecks)
        {
            _exceptionFromWhenAction = exceptionFromWhenAction
                                       ?? throw new ArgumentNullException(nameof(exceptionFromWhenAction));
            if (failedExceptionChecks.IsEmpty)
                throw new ArgumentException($"{nameof(failedExceptionChecks)} can not be empty");
            _failedExceptionChecks = failedExceptionChecks;
        }

        public override void ThrowOnErrors() =>
            throw new ExceptionChecksFailed(_exceptionFromWhenAction, _failedExceptionChecks);

        public bool Equals(UnexpectedExceptionInWhenAction other) =>
            !(other is null) &&
            _exceptionFromWhenAction.Equals(other._exceptionFromWhenAction) &&
            _failedExceptionChecks.SequenceEqual(other._failedExceptionChecks);

        public override bool Equals(object obj) => obj is UnexpectedExceptionInWhenAction other && Equals(other);

        public override int GetHashCode() =>
            (_exceptionFromWhenAction, HashCode.Combine(_failedExceptionChecks)).GetHashCode();
    }
}