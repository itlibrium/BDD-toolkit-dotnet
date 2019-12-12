using System;
using FluentAssertions;
using FluentAssertions.Execution;
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
        public void ExceptionInGivenActionsIsCaught()
        {
            var exception = new InvalidOperationException();
            FirstGivenActionThrows(exception);
            var scenarioTest = CreateScenarioWithResultsCheck().Test;

            var testResult = scenarioTest.Run();

            TestShouldFailWithSingleException(testResult, exception);
            WhenActionShouldNotBeExecuted();
            AllThenActionsShouldNotBeExecuted();
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
        public void AllThenActionsAreExecutedEvenIfExceptionWasThrownInWhenAction()
        {
            var exception = new InvalidOperationException();
            WhenActionThrows(exception);
            var scenarioTest = CreateScenarioWithResultsCheck().Test;

            scenarioTest.Run();
            
            AllThenActionsShouldBeExecutedOnce();
        }

        [Fact]
        public void SecondThenActionIsInvokedEvenIfFirstAssertFailed()
        {
            var exception = new AssertionFailedException("Assertion failed");
            FirstThenActionThrows(exception);
            var scenarioTest = CreateScenarioWithResultsCheck().Test;
            
            var testResult = scenarioTest.Run();
            
            TestShouldFailWithSingleException(testResult, exception);
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
            
            TestShouldFailWithSingleException(testResult, exception1);
        }

        [Fact]
        public void TestPassesWhenExceptionIsThrownAndExplicitExceptionCheckIsMade()
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
        public void TestFailsWhenExceptionIsThrownAndNoExplicitExceptionCheckIsMade()
        {
            var exception = new InvalidOperationException();
            WhenActionThrows(exception);
            var scenarioTest = CreateScenarioWithResultsCheck().Test;
            
            var testResult = scenarioTest.Run();
            
            TestShouldFailWithSingleException(testResult, exception);
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
        
        private static void TestShouldFailWithSingleException(TestResult testResult, Exception exception)
        {
            testResult.IsSuccessful.Should().BeFalse();
            testResult.Exceptions.Length.Should().Be(1);
            testResult.Exceptions[0].Should().Be(exception);
        }
        
        private static void TestShouldFailWithExceptions(TestResult testResult, params Exception[] exceptions)
        {
            testResult.IsSuccessful.Should().BeFalse();
            testResult.Exceptions.Should().BeEquivalentTo(exceptions);
        }
        
        private void AllThenActionsShouldBeExecutedOnce()
        {
            ContextMock.Verify(c => c.Result1IsAsExpected(), Times.Once);
            ContextMock.Verify(c => c.Result2IsAsExpected(), Times.Once);
        }
        
        private void AllThenActionsShouldNotBeExecuted()
        {
            ContextMock.Verify(c => c.Result1IsAsExpected(), Times.Never);
            ContextMock.Verify(c => c.Result2IsAsExpected(), Times.Never);
        }
        
        private static void TestShouldPass(in TestResult testResult) => testResult.IsSuccessful.Should().BeTrue();

        
    }
}