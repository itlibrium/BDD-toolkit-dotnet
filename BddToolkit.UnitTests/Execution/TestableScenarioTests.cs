using System;
using System.Collections.Immutable;
using System.Threading;
using FluentAssertions;
using FluentAssertions.Execution;
using ITLIBRIUM.BddToolkit.Docs;
using ITLIBRIUM.BddToolkit.Syntax.Scenarios;
using ITLIBRIUM.BddToolkit.Tests;
using ITLIBRIUM.BddToolkit.Tests.Results;
using Moq;
using Xunit;

namespace ITLIBRIUM.BddToolkit.Execution
{
    public class TestableScenarioTests : TestsBase
    {
        private readonly Mock<DocPublisher> _docPublisherMock;

        public TestableScenarioTests() => _docPublisherMock = new Mock<DocPublisher>();

        [Fact]
        public void ScenarioIsPublishedUnchanged()
        {
            var testableScenario = CreateScenarioWithResultsCheck();

            Publish(testableScenario);

            _docPublisherMock.Verify(p => p
                    .Append(It.Is<Scenario>(scenario => scenario.Equals(testableScenario.Scenario)),
                        It.IsAny<TestStatus>(),
                        It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public void ScenarioIsPublishedWithUnknownTestStatus()
        {
            var testableScenario = CreateScenarioWithResultsCheck();

            Publish(testableScenario);

            _docPublisherMock.Verify(p => p
                    .Append(It.IsAny<Scenario>(),
                        TestStatus.Unknown,
                        It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public void TestedScenarioHasScenarioUnchanged()
        {
            var testableScenario = CreateScenarioWithResultsCheck();

            var testedScenario = testableScenario.RunTest();

            testedScenario.Scenario.Should().Be(testableScenario.Scenario);
        }

        [Fact]
        public void TestedScenarioForPassingTestHasPassedTestResult()
        {
            var testableScenario = CreateScenarioWithResultsCheck();

            var testedScenario = testableScenario.RunTest();

            testedScenario.TestResult.Should().Be(TestResult.Passed());
        }

        [Fact]
        public void TestedScenarioForTestThrowingInGivenActionsHasAppropriateResult()
        {
            var exception = new InvalidOperationException();
            FirstGivenActionThrows(exception);
            var testableScenario = CreateScenarioWithResultsCheck();

            var testedScenario = testableScenario.RunTest();

            testedScenario.TestResult.Should().Be(new ExceptionInGivenAction(exception));
        }

        [Fact]
        public void TestedScenarioForTestThrowingInWhenActionsWithoutExceptionCheckHasAppropriateResult()
        {
            var exception = new InvalidOperationException();
            WhenActionThrows(exception);
            var testableScenario = CreateScenarioWithResultsCheck();

            var testedScenario = testableScenario.RunTest();

            testedScenario.TestResult.Should().Be(new UncheckedExceptionInWhenAction(exception));
        }

        [Fact]
        public void TestedScenarioForTestThrowingUnexpectedExceptionInWhenActionsHasAppropriateResult()
        {
            var exception1 = new InvalidOperationException();
            WhenActionThrows(exception1);
            var exception2 = new AssertionFailedException("error");
            ExceptionCheckThrows(exception2);
            var testableScenario = CreateScenarioWithResultsAndExceptionCheck();

            var testedScenario = testableScenario.RunTest();

            testedScenario.TestResult.Should()
                .Be(new UnexpectedExceptionInWhenAction(exception1, ImmutableArray.Create<Exception>(exception2)));
        }

        [Fact]
        public void TestedScenarioForFailingTestHasFailedTestResult()
        {
            var exception1 = new InvalidOperationException();
            FirstThenActionThrows(exception1);
            var exception2 = new InvalidOperationException();
            SecondThenActionThrows(exception2);
            var testableScenario = CreateScenarioWithResultsCheck();

            var testedScenario = testableScenario.RunTest();

            testedScenario.TestResult.Should()
                .Be(TestResult.Failed(ImmutableArray.Create<Exception>(exception1, exception2)));
        }

        private void Publish(TestableScenario testableScenario) =>
            testableScenario.PublishDoc(_docPublisherMock.Object, CancellationToken.None);
    }
}