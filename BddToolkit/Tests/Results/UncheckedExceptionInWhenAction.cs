using System;
using ITLIBRIUM.BddToolkit.Tests.Results.Exceptions;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Tests.Results
{
    internal class UncheckedExceptionInWhenAction : TestResult, IEquatable<UncheckedExceptionInWhenAction>
    {
        private readonly Exception _exception;

        protected override bool IsSuccessful => false;

        public UncheckedExceptionInWhenAction([NotNull] Exception exception) =>
            _exception = exception ?? throw new ArgumentNullException(nameof(exception));

        public override void ThrowOnErrors() => throw new UncheckedExceptionInWhenActionFound(_exception);

        public bool Equals(UncheckedExceptionInWhenAction other) => 
            !(other is null) &&
            _exception.Equals(other._exception);

        public override bool Equals(object obj) => obj is UncheckedExceptionInWhenAction other && Equals(other);

        public override int GetHashCode() => _exception.GetHashCode();
    }
}