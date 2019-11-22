using System;
using FluentAssertions;
using ITLIBRIUM.BddToolkit.Execution;
using Moq;
using Xunit;

namespace ITLIBRIUM.BddToolkit.Tests
{
    public class ScenarioTestTests
    {
        [Fact]
        public void AllGivenActionsAreInvokedOnce()
        {
            var contextMock = new Mock<Context>();

            TestScenario(contextMock.Object);

            contextMock.Verify(c => c.FirstFact(), Times.Once);
            contextMock.Verify(c => c.SecondFact(), Times.Once);
        }

        [Fact]
        public void GivenActionsAreNotRequired()
        {
            var contextMock = new Mock<Context>();
            Action test = () => Bdd.Scenario(contextMock.Object)
                .When(c => c.SomethingIsDone())
                .Then(c => c.Result1IsAsExpected())
                .Create()
                .RunTest()
                .ThrowOnErrors();
            test.Should().NotThrow();
        }

        [Fact]
        public void WhenActionIsInvokedOnce()
        {
            var contextMock = new Mock<Context>();

            TestScenario(contextMock.Object);

            contextMock.Verify(c => c.SomethingIsDone(), Times.Once);
        }

        [Fact]
        public void AllThenActionsAreInvokedOnce()
        {
            var contextMock = new Mock<Context>();

            TestScenario(contextMock.Object);

            contextMock.Verify(c => c.Result1IsAsExpected(), Times.Once);
            contextMock.Verify(c => c.Result2IsAsExpected(), Times.Once);
        }

        [Fact]
        public void AllThenActionsAreInvokedEvenIfExceptionWasThrown()
        {
            var contextMock = new Mock<Context>();
            contextMock.Setup(c => c.SomethingIsDone()).Throws<Exception>();

            var testedScenario = CreateScenario(contextMock.Object).RunTest();
            testedScenario.TestResult.IsSuccessful.Should().BeFalse();
            contextMock.Verify(c => c.Result1IsAsExpected(), Times.Once);
            contextMock.Verify(c => c.Result2IsAsExpected(), Times.Once);
        }

        [Fact]
        public void SecondThenActionIsInvokedEvenIfFirstAssertFailed()
        {
            var contextMock = new Mock<Context>();
            contextMock.Setup(c => c.Result1IsAsExpected()).Throws<Exception>();

            var testedScenario = CreateScenario(contextMock.Object).RunTest();
            testedScenario.TestResult.IsSuccessful.Should().BeFalse();
            contextMock.Verify(c => c.Result1IsAsExpected(), Times.Once);
            contextMock.Verify(c => c.Result2IsAsExpected(), Times.Once);
        }

        [Fact]
        public void TestPassesWhenExceptionIsThrownAndExplicitAssertionIsMade()
        {
            var contextMock = new Mock<Context>();
            contextMock.Setup(c => c.SomethingIsDone()).Throws<Exception>();

            Action test = () => Bdd.Scenario(contextMock.Object)
                .When(c => c.SomethingIsDone())
                .Then((c, r) => c.ExceptionIsThrown(r))
                .Create()
                .RunTest()
                .ThrowOnErrors();
            test.Should().NotThrow();

            Action test2 = () => Bdd.Scenario(contextMock.Object)
                .When(c => c.SomethingIsDone())
                .Then(c => c.Result1IsAsExpected())
                .And((c, r) => c.ExceptionIsThrown(r))
                .Create()
                .RunTest()
                .ThrowOnErrors();
            test2.Should().NotThrow();
        }

        [Fact]
        public void TestFailsWhenExceptionIsThrownAndNoExplicitAssertionIsMade()
        {
            var contextMock = new Mock<Context>();
            contextMock.Setup(c => c.SomethingIsDone()).Throws<Exception>();

            var testedScenario = Bdd.Scenario(contextMock.Object)
                .Given(c => c.FirstFact())
                .When(c => c.SomethingIsDone())
                .Then(c => c.Result1IsAsExpected())
                .Create()
                .RunTest();
            testedScenario.TestResult.IsSuccessful.Should().BeFalse();
        }

        private static void TestScenario(Context context) =>
            Bdd.Scenario(context)
                .Given(c => c.FirstFact())
                .And(c => c.SecondFact())
                .When(c => c.SomethingIsDone())
                .Then(c => c.Result1IsAsExpected())
                .And(c => c.Result2IsAsExpected())
                .Create()
                .RunTest();

        private static TestableScenario CreateScenario(Context context, string name = null) =>
            Bdd.Scenario(context)
                .Name(name)
                .Given(c => c.FirstFact())
                .And(c => c.SecondFact())
                .When(c => c.SomethingIsDone())
                .Then(c => c.Result1IsAsExpected())
                .And(c => c.Result2IsAsExpected())
                .Create();

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