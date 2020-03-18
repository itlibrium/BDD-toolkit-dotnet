using System;
using ITLIBRIUM.BddToolkit.Tests.Results.Exceptions;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Tests.Results
{
    internal class ExceptionInGivenAction : TestResult, IEquatable<ExceptionInGivenAction>
    {
        private readonly Exception _exception;

        protected override bool IsSuccessful => false;
        
        public ExceptionInGivenAction([NotNull] Exception exception) => 
            _exception = exception ?? throw new ArgumentNullException(nameof(exception));

        public override void ThrowOnErrors() => throw new GivenActionFailed(_exception);

        public bool Equals(ExceptionInGivenAction other) => _exception.Equals(other?._exception);

        public override bool Equals(object obj) => obj is ExceptionInGivenAction other && Equals(other);

        public override int GetHashCode() => _exception.GetHashCode();
    }
}