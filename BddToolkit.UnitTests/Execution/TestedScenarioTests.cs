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
    public class TestedScenarioTests : TestsBase
    {
        private readonly Mock<DocPublisher> _docPublisherMock;

        public TestedScenarioTests() => _docPublisherMock = new Mock<DocPublisher>();
        
        [Fact]
        public void ScenarioIsPublishedUnchanged()
        {
            var testedScenario = CreateScenarioWithResultsCheck().RunTest();

            Publish(testedScenario);
            
            _docPublisherMock.Verify(p => p
                .Append(It.Is<Scenario>(scenario => scenario.Equals(testedScenario.Scenario)), 
                    It.IsAny<TestStatus>(), 
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
        
        [Fact]
        public void ScenarioWithPassedTestIsPublishedWithPassedTestStatus()
        {
            var testedScenario = CreateScenarioWithResultsCheck().RunTest();

            Publish(testedScenario);
            
            _docPublisherMock.Verify(p => p
                .Append(It.IsAny<Scenario>(), 
                    TestStatus.Passed, 
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
        
        [Fact]
        public void ScenarioWithFailedTestIsPublishedWithFailedTestStatus()
        {
            FirstThenActionThrows(new InvalidOperationException());
            var testedScenario = CreateScenarioWithResultsCheck().RunTest();

            Publish(testedScenario);
            
            _docPublisherMock.Verify(p => p
                .Append(It.IsAny<Scenario>(), 
                    TestStatus.Failed, 
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
        
        [Fact]
        public void ExceptionIsNotThrownForPassingTest()
        {
            var testedScenario = CreateScenarioWithResultsCheck().RunTest();
            
            Action action = () => testedScenario.ThrowOnErrors();
            
            action.Should().NotThrow();
        }
        
        [Fact]
        public void AggregateAssertExceptionIsThrownForFailedTest()
        {
            var exception1 = new InvalidOperationException();
            FirstThenActionThrows(exception1);
            var exception2 = new InvalidOperationException();
            SecondThenActionThrows(exception2);

            var testedScenario = CreateScenarioWithResultsCheck().RunTest();
            
            Action action = () => testedScenario.ThrowOnErrors();

            action.Should().Throw<AggregateAssertException>()
                .And.Exceptions.Should().BeEquivalentTo(exception1, exception2);
        }
        
        private void Publish(TestedScenario testedScenario) => 
            testedScenario.PublishDoc(_docPublisherMock.Object, CancellationToken.None);
    }
}