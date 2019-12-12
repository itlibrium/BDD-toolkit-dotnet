using System;
using ITLIBRIUM.BddToolkit.Docs;
using ITLIBRIUM.BddToolkit.Execution;
using Moq;

namespace ITLIBRIUM.BddToolkit
{
    public class TestsBase
    {
        protected Mock<Context> ContextMock { get; }
        protected Mock<DocPublisher> DocPublisherMock { get; }

        protected TestsBase()
        {
            ContextMock = new Mock<Context>();
            DocPublisherMock = new Mock<DocPublisher>();
            Bdd.Configure(configuration => configuration.DocPublisher = DocPublisherMock.Object);
        }

        protected void FirstGivenActionThrows(Exception exception) => 
            ContextMock.Setup(c => c.FirstFact()).Throws(exception);

        protected void WhenActionThrows(Exception exception) => 
            ContextMock.Setup(c => c.SomethingIsDone()).Throws(exception);

        protected void FirstThenActionThrows(Exception exception) => 
            ContextMock.Setup(c => c.Result1IsAsExpected()).Throws(exception);

        protected void SecondThenActionThrows(Exception exception) => 
            ContextMock.Setup(c => c.Result2IsAsExpected()).Throws(exception);

        protected TestableScenario CreateScenarioWithResultsCheck() => Bdd.Scenario(ContextMock.Object)
            .Given(c => c.FirstFact())
            .And(c => c.SecondFact())
            .When(c => c.SomethingIsDone())
            .Then(c => c.Result1IsAsExpected())
            .And(c => c.Result2IsAsExpected())
            .Create();
        
        protected TestableScenario CreateScenarioWithExceptionCheck() => Bdd.Scenario(ContextMock.Object)
            .Given(c => c.FirstFact())
            .And(c => c.SecondFact())
            .When(c => c.SomethingIsDone())
            .Then((c, r) => c.ExceptionIsThrown(r))
            .Create();
        
        protected TestableScenario CreateScenarioWithResultsAndExceptionCheck() => Bdd.Scenario(ContextMock.Object)
            .Given(c => c.FirstFact())
            .And(c => c.SecondFact())
            .When(c => c.SomethingIsDone())
            .Then(c => c.Result1IsAsExpected())
            .And(c => c.Result2IsAsExpected())
            .And((c, r) => c.ExceptionIsThrown(r))
            .Create();
        
        // ReSharper disable once MemberCanBeProtected.Global - required by Moq
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