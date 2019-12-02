using System;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using Xunit;

namespace ITLIBRIUM.BddToolkit.Tests
{
    public class ScenarioTestTests
    {
        private readonly Mock<Context> _contextMock;

        public ScenarioTestTests() => _contextMock = new Mock<Context>();

        [Fact]
        public void AllGivenActionsAreExecutedOnce()
        {
            var scenarioTest = CreateDefaultScenarioTest(_contextMock.Object);

            scenarioTest.Run();

            AllGivenActionsShouldBeExecutedOnce();
        }

        [Fact]
        public void GivenActionsAreNotRequired()
        {
            var scenarioTest = Bdd.Scenario(_contextMock.Object)
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
            _contextMock.Setup(c => c.FirstFact()).Throws(exception);
            var scenarioTest = CreateDefaultScenarioTest(_contextMock.Object);

            var testResult = scenarioTest.Run();

            TestShouldFailWithSingleException(testResult, exception);
            WhenActionShouldNotBeExecuted();
            AllThenActionsShouldNotBeExecuted();
        }

        [Fact]
        public void WhenActionIsExecutedOnce()
        {
            var scenarioTest = CreateDefaultScenarioTest(_contextMock.Object);

            scenarioTest.Run();

            WhenActionShouldBeExecutedOnce();
        }

        [Fact]
        public void AllThenActionsAreExecutedOnce()
        {
            var scenarioTest = CreateDefaultScenarioTest(_contextMock.Object);

            scenarioTest.Run();

            AllThenActionsShouldBeExecutedOnce();
        }

        [Fact]
        public void AllThenActionsAreExecutedEvenIfExceptionWasThrownInWhenAction()
        {
            var exception = new InvalidOperationException();
            _contextMock.Setup(c => c.SomethingIsDone()).Throws(exception);

            CreateDefaultScenarioTest(_contextMock.Object).Run();
            
            AllThenActionsShouldBeExecutedOnce();
        }

        [Fact]
        public void SecondThenActionIsInvokedEvenIfFirstAssertFailed()
        {
            var exception = new AssertionFailedException("Assertion failed");
            _contextMock.Setup(c => c.Result1IsAsExpected()).Throws(exception);
            var scenarioTest = CreateDefaultScenarioTest(_contextMock.Object);
            
            var testResult = scenarioTest.Run();
            
            TestShouldFailWithSingleException(testResult, exception);
            AllThenActionsShouldBeExecutedOnce();
        }
        
        [Fact]
        public void AllExceptionsFromThenActionsAreReportedInTestResult()
        {
            var exception1 = new InvalidOperationException();
            _contextMock.Setup(c => c.Result1IsAsExpected()).Throws(exception1);
            var exception2 = new InvalidOperationException();
            _contextMock.Setup(c => c.Result2IsAsExpected()).Throws(exception2);
            var scenarioTest = CreateDefaultScenarioTest(_contextMock.Object);
            
            var testResult = scenarioTest.Run();
            
            testResult.IsSuccessful.Should().BeFalse();
            testResult.Exceptions.Length.Should().Be(2);
            testResult.Exceptions[0].Should().Be(exception1);
            testResult.Exceptions[1].Should().Be(exception2);
        }
        
        [Fact]
        public void ExceptionFromWhenActionIsNotReportedForFailedTestsInTestResultWhenExceptionCheckIsMade()
        {
            var exception0 = new InvalidOperationException();
            _contextMock.Setup(c => c.SomethingIsDone()).Throws(exception0);
            var exception1 = new InvalidOperationException();
            _contextMock.Setup(c => c.Result1IsAsExpected()).Throws(exception1);
            var scenarioTest = Bdd.Scenario(_contextMock.Object)
                .When(c => c.SomethingIsDone())
                .Then(c => c.Result1IsAsExpected())
                .And((c, r) => c.ExceptionIsThrown(r))
                .Create()
                .Test;
            
            var testResult = scenarioTest.Run();
            
            TestShouldFailWithSingleException(testResult, exception1);
        }

        [Fact]
        public void TestPassesWhenExceptionIsThrownAndExplicitAssertionIsMade()
        {
            var exception = new InvalidOperationException();
            _contextMock.Setup(c => c.SomethingIsDone()).Throws(exception);
            var firstScenarioTest = Bdd.Scenario(_contextMock.Object)
                .When(c => c.SomethingIsDone())
                .Then((c, r) => c.ExceptionIsThrown(r))
                .Create()
                .Test;
            var secondScenarioTest = Bdd.Scenario(_contextMock.Object)
                .When(c => c.SomethingIsDone())
                .Then(c => c.Result1IsAsExpected())
                .And((c, r) => c.ExceptionIsThrown(r))
                .Create()
                .Test;
            
            var firstTestResult = firstScenarioTest.Run();
            var secondTestResult = secondScenarioTest.Run();
            
            firstTestResult.IsSuccessful.Should().BeTrue();
            secondTestResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void TestFailsWhenExceptionIsThrownAndNoExplicitAssertionIsMade()
        {
            var exception = new InvalidOperationException();
            _contextMock.Setup(c => c.SomethingIsDone()).Throws(exception);
            var scenarioTest = Bdd.Scenario(_contextMock.Object)
                .Given(c => c.FirstFact())
                .When(c => c.SomethingIsDone())
                .Then(c => c.Result1IsAsExpected())
                .Create()
                .Test;
            
            var testResult = scenarioTest.Run();
            
            TestShouldFailWithSingleException(testResult, exception);
        }

        

        private static ScenarioTest CreateDefaultScenarioTest(Context context) => Bdd.Scenario(context)
            .Given(c => c.FirstFact())
            .And(c => c.SecondFact())
            .When(c => c.SomethingIsDone())
            .Then(c => c.Result1IsAsExpected())
            .And(c => c.Result2IsAsExpected())
            .Create()
            .Test;
        
        private void AllGivenActionsShouldBeExecutedOnce()
        {
            _contextMock.Verify(c => c.FirstFact(), Times.Once);
            _contextMock.Verify(c => c.SecondFact(), Times.Once);
        }
        
        private void WhenActionShouldBeExecutedOnce()
        {
            _contextMock.Verify(c => c.SomethingIsDone(), Times.Once);
        }
        
        private void WhenActionShouldNotBeExecuted()
        {
            _contextMock.Verify(c => c.SomethingIsDone(), Times.Never);
        }
        
        private static void TestShouldFailWithSingleException(TestResult testResult, Exception exception)
        {
            testResult.IsSuccessful.Should().BeFalse();
            testResult.Exceptions.Length.Should().Be(1);
            testResult.Exceptions[0].Should().Be(exception);
        }
        
        private void AllThenActionsShouldBeExecutedOnce()
        {
            _contextMock.Verify(c => c.Result1IsAsExpected(), Times.Once);
            _contextMock.Verify(c => c.Result2IsAsExpected(), Times.Once);
        }
        
        private void AllThenActionsShouldNotBeExecuted()
        {
            _contextMock.Verify(c => c.Result1IsAsExpected(), Times.Never);
            _contextMock.Verify(c => c.Result2IsAsExpected(), Times.Never);
        }

        // ReSharper disable once MemberCanBePrivate.Global - required by Moq
        public interface Context
        {
            void FirstFact();
            void SecondFact();

            void SomethingIsDone();

            void Result1IsAsExpected();
            void Result2IsAsExpected();

            void ExceptionIsThrown(Result result);
        }
    }
}