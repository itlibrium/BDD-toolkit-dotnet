using System;
using System.Threading;
using FluentAssertions;
using ITLIBRIUM.BddToolkit.Docs;
using ITLIBRIUM.BddToolkit.Execution;
using ITLIBRIUM.BddToolkit.Syntax.Scenarios;
using ITLIBRIUM.BddToolkit.Tests;
using Moq;
using Xunit;

namespace ITLIBRIUM.BddToolkit.Builders
{
    public class ScenarioBuilderTests : TestsBase
    {
        private readonly Mock<DocPublisher> _docPublisherMock;

        public ScenarioBuilderTests()
        {
            _docPublisherMock = new Mock<DocPublisher>();
            Bdd.Configure(configuration => configuration.DocPublisher = _docPublisherMock.Object);
        }
        
        [Fact]
        public void ScenarioIsPublished()
        {
            TestScenario();
            
            _docPublisherMock.Verify(p => p
                .Append(It.IsAny<Scenario>(), 
                    TestStatus.Passed, 
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
        
        [Fact]
        public void ExceptionIsNotThrownForPassingTest()
        {
            Action action = TestScenario;
            
            action.Should().NotThrow();
        }

        [Fact]
        public void ExceptionIsThrownForFailingTest()
        {
            FirstThenActionThrows(new InvalidOperationException());
            
            Action action = TestScenario;
            
            action.Should().ThrowExactly<AggregateAssertException>();
        }

        private void TestScenario() => Bdd.Scenario(ContextMock.Object)
            .Given(c => c.FirstFact())
            .And(c => c.SecondFact())
            .When(c => c.SomethingIsDone())
            .Then(c => c.Result1IsAsExpected())
            .And(c => c.Result2IsAsExpected())
            .Test();
    }
}