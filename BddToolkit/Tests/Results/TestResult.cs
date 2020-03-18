using System;
using System.Collections.Immutable;

namespace ITLIBRIUM.BddToolkit.Tests.Results
{
    public abstract class TestResult
    {
        protected abstract bool IsSuccessful { get; }

        public static TestResult Passed() => Results.Passed.Instance;

        public static TestResult ExceptionInGivenAction(Exception exceptionFromGivenAction) => 
            new ExceptionInGivenAction(exceptionFromGivenAction);

        public static TestResult UncheckedExceptionInWhenAction(Exception exceptionFromWhenAction) => 
            new UncheckedExceptionInWhenAction(exceptionFromWhenAction);
        
        public static TestResult UnexpectedExceptionInWhenAction(Exception exceptionFromWhenAction, 
            ImmutableArray<Exception> failedExceptionChecks) =>
            new UnexpectedExceptionInWhenAction(exceptionFromWhenAction, failedExceptionChecks);

        public static TestResult Failed(ImmutableArray<Exception> exceptionsFromThenActions) =>
            new Failed(exceptionsFromThenActions);

        public TestStatus ToTestStatus() => IsSuccessful ? TestStatus.Passed : TestStatus.Failed;

        public abstract void ThrowOnErrors();
    }
}