using System;
using System.Collections.Immutable;

namespace ITLIBRIUM.BddToolkit.Tests
{
    public readonly struct TestResult
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
    }
}