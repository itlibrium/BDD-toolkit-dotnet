using System;
using System.Collections.Immutable;
using FluentAssertions;
using FluentAssertions.Execution;
using ITLIBRIUM.BddToolkit.Tests.Results;
using Moq;
using Xunit;

namespace ITLIBRIUM.BddToolkit.Tests
{
    public class ScenarioTestTests : TestsBase
    {
        [Fact]
        public void AllGivenActionsAreExecutedOnce()
        {
            var scenarioTest = CreateScenarioWithResultsCheck().Test;

            scenarioTest.Run();

            AllGivenActionsShouldBeExecutedOnce();
        }

        [Fact]
        public void GivenActionsAreNotRequired()
        {
            var scenarioTest = Bdd.Scenario(ContextMock.Object)
                .When(c => c.SomethingIsDone())
                .Then(c => c.Result1IsAsExpected())
                .Create()
                .Test;

            Action test = () => scenarioTest.Run();

            test.Should().NotThrow();
        }

        [Fact]
        public void ExceptionInGivenActionsStopsTestExecution()
        {
            var exception = new InvalidOperationException();
            FirstGivenActionThrows(exception);
            var scenarioTest = CreateScenarioWithResultsCheck().Test;

            var testResult = scenarioTest.Run();

            TestShouldFailDueToExceptionInGivenActions(testResult, exception);
            WhenActionShouldNotBeExecuted();
            ThenActionsShouldNotBeExecuted();
        }

        [Fact]
        public void WhenActionsAreNotRequired()
        {
            var scenarioTest = Bdd.Scenario(ContextMock.Object)
                .Given(c => c.FirstFact())
                .Then(c => c.Result1IsAsExpected())
                .Create()
                .Test;

            Action test = () => scenarioTest.Run();

            test.Should().NotThrow();
        }

        [Fact]
        public void WhenActionIsExecutedOnce()
        {
            var scenarioTest = CreateScenarioWithResultsCheck().Test;

            scenarioTest.Run();

            WhenActionShouldBeExecutedOnce();
        }

        [Fact]
        public void AllThenActionsAreExecutedOnce()
        {
            var scenarioTest = CreateScenarioWithResultsCheck().Test;

            scenarioTest.Run();

            AllThenActionsShouldBeExecutedOnce();
        }

        [Fact]
        public void AllThenActionsAreExecutedIfExceptionWasThrownInWhenActionAndExceptionCheckIsMade()
        {
            var exception = new InvalidOperationException();
            WhenActionThrows(exception);
            var scenarioTest = CreateScenarioWithResultsAndExceptionCheck().Test;

            scenarioTest.Run();

            AllThenActionsShouldBeExecutedOnce();
        }

        [Fact]
        public void ThenActionsAreNotExecutedIfExceptionWasThrownInWhenActionAndExceptionCheckIsNotMade()
        {
            var exception = new InvalidOperationException();
            WhenActionThrows(exception);
            var scenarioTest = CreateScenarioWithResultsCheck().Test;

            scenarioTest.Run();

            ThenActionsShouldNotBeExecuted();
        }

        [Fact]
        public void ExceptionChecksAreExecutedIfExceptionWasNotThrownInWhenAction()
        {
            var scenarioTest = CreateScenarioWithResultsAndExceptionCheck().Test;

            scenarioTest.Run();

            ExceptionChecksShouldBeExecutedOnce();
        }

        [Fact]
        public void SecondThenActionIsInvokedEvenIfFirstAssertFailed()
        {
            var exception = new AssertionFailedException("Assertion failed");
            FirstThenActionThrows(exception);
            var scenarioTest = CreateScenarioWithResultsCheck().Test;

            var testResult = scenarioTest.Run();

            TestShouldFailWithExceptions(testResult, exception);
            AllThenActionsShouldBeExecutedOnce();
        }

        [Fact]
        public void AllExceptionsFromThenActionsAreReportedInTestResult()
        {
            var exception1 = new AssertionFailedException("Assertion 1 failed");
            FirstThenActionThrows(exception1);
            var exception2 = new AssertionFailedException("Assertion 2 failed");
            SecondThenActionThrows(exception2);
            var scenarioTest = CreateScenarioWithResultsCheck().Test;

            var testResult = scenarioTest.Run();

            TestShouldFailWithExceptions(testResult, exception1, exception2);
        }

        [Fact]
        public void ExceptionFromWhenActionIsNotReportedForFailedTestsInTestResultWhenExceptionCheckIsMade()
        {
            var exception0 = new InvalidOperationException();
            WhenActionThrows(exception0);
            var exception1 = new InvalidOperationException();
            FirstThenActionThrows(exception1);
            var scenarioTest = CreateScenarioWithResultsAndExceptionCheck().Test;

            var testResult = scenarioTest.Run();

            TestShouldFailWithExceptions(testResult, exception1);
        }

        [Fact]
        public void TestPassesWhenExceptionIsThrownAndExceptionCheckIsMade()
        {
            var exception = new InvalidOperationException();
            WhenActionThrows(exception);
            var firstScenarioTest = CreateScenarioWithExceptionCheck().Test;
            var secondScenarioTest = CreateScenarioWithResultsAndExceptionCheck().Test;

            var firstTestResult = firstScenarioTest.Run();
            var secondTestResult = secondScenarioTest.Run();

            TestShouldPass(firstTestResult);
            TestShouldPass(secondTestResult);
        }

        [Fact]
        public void TestFailsWhenExceptionIsThrownAndExceptionCheckIsNotMade()
        {
            var exception = new InvalidOperationException();
            WhenActionThrows(exception);
            var scenarioTest = CreateScenarioWithResultsCheck().Test;

            var testResult = scenarioTest.Run();

            TestShouldFailDueToUncheckedExceptionInWhenAction(testResult, exception);
        }

        private void AllGivenActionsShouldBeExecutedOnce()
        {
            ContextMock.Verify(c => c.FirstFact(), Times.Once);
            ContextMock.Verify(c => c.SecondFact(), Times.Once);
        }

        private void WhenActionShouldBeExecutedOnce()
        {
            ContextMock.Verify(c => c.SomethingIsDone(), Times.Once);
        }

        private void WhenActionShouldNotBeExecuted()
        {
            ContextMock.Verify(c => c.SomethingIsDone(), Times.Never);
        }

        private static void TestShouldFailDueToExceptionInGivenActions(TestResult testResult, Exception exception) =>
            testResult.Should().Be(new ExceptionInGivenAction(exception));

        private static void TestShouldFailDueToUncheckedExceptionInWhenAction(TestResult testResult,
            Exception exception) =>
            testResult.Should().Be(new UncheckedExceptionInWhenAction(exception));

        private static void TestShouldFailWithExceptions(TestResult testResult, params Exception[] exceptions) =>
            testResult.Should().Be(new Failed(exceptions.ToImmutableArray()));

        private void AllThenActionsShouldBeExecutedOnce()
        {
            ContextMock.Verify(c => c.Result1IsAsExpected(), Times.Once);
            ContextMock.Verify(c => c.Result2IsAsExpected(), Times.Once);
        }

        private void ThenActionsShouldNotBeExecuted()
        {
            ContextMock.Verify(c => c.Result1IsAsExpected(), Times.Never);
            ContextMock.Verify(c => c.Result2IsAsExpected(), Times.Never);
        }

        private void ExceptionChecksShouldBeExecutedOnce() =>
            ContextMock.Verify(c => c.ExceptionIsThrown(It.IsAny<Result>()), Times.Once);

        private static void TestShouldPass(TestResult testResult) => testResult.Should().Be(Passed.Instance);
    }
}