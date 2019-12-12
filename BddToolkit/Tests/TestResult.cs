using System;
using System.Collections.Immutable;
using System.Linq;

namespace ITLIBRIUM.BddToolkit.Tests
{
    public readonly struct TestResult : IEquatable<TestResult>
    {
        public bool IsSuccessful { get; }
        public ImmutableArray<Exception> Exceptions { get; }

        public static TestResult Passed() => 
            new TestResult(true, ImmutableArray<Exception>.Empty);

        public static TestResult Failed(Exception exception) => 
            new TestResult(false, ImmutableArray.Create(exception));

        public static TestResult Failed(ImmutableArray<Exception> exceptions) => 
            new TestResult(false, exceptions);

        private TestResult(bool isSuccessful, ImmutableArray<Exception> exceptions)
        {
            IsSuccessful = isSuccessful;
            Exceptions = exceptions;
        }

        public TestStatus ToTestStatus() => IsSuccessful ? TestStatus.Passed : TestStatus.Failed;

        public bool Equals(TestResult other) => 
            IsSuccessful == other.IsSuccessful && 
            Exceptions.SequenceEqual(other.Exceptions);
        public override bool Equals(object obj) => obj is TestResult other && Equals(other);
        public override int GetHashCode() => (IsSuccessful, HashCode.Combine(Exceptions)).GetHashCode();
    }
}