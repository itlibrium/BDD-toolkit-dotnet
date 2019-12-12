using System;
using System.Threading;
using FluentAssertions;
using ITLIBRIUM.BddToolkit.Docs;
using ITLIBRIUM.BddToolkit.Syntax.Scenarios;
using ITLIBRIUM.BddToolkit.Tests;
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
        public void TestedScenarioForFailingTestHasFailedTestResult()
        {
            var exception = new InvalidOperationException();
            WhenActionThrows(exception);
            var testableScenario = CreateScenarioWithResultsCheck();

            var testedScenario = testableScenario.RunTest();

            testedScenario.TestResult.Should().Be(TestResult.Failed(exception));
        }
        
        private void Publish(TestableScenario testableScenario) => 
            testableScenario.PublishDoc(_docPublisherMock.Object, CancellationToken.None);
    }
}