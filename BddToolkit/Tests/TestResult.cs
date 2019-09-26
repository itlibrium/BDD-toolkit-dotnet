using System;
using System.Collections.Generic;
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

        public static TestResult Failed(IEnumerable<Exception> exceptions) => 
            new TestResult(false, exceptions.ToImmutableArray());

        private TestResult(bool isSuccessful, ImmutableArray<Exception> exceptions)
        {
            IsSuccessful = isSuccessful;
            Exceptions = exceptions;
        }
    }
}