using System;
using FluentAssertions;
using ITLIBRIUM.BddToolkit.Execution;
using Moq;
using Xunit;

namespace ITLIBRIUM.BddToolkit.Tests.Scenarios
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

        // TODO: do testów DocFormatter
//        [Fact]
//        public void DefaultTitleIsEqualToHumanizedMethodName()
//        {
//            var contextMock = new Mock<Context>();
//            var runner = Bdd.Scenario(contextMock.Object)
//                .Given(c => c.FirstFact())
//                .And(c => c.SecondFact())
//                .When(c => c.SomethingIsDone())
//                .Then(c => c.Result1IsAsExpected())
//                .And(c => c.Result2IsAsExpected())
//                .Create();
//
//            runner.Scenario.Name.Should().Be("Default title is equal to humanized method name");
//        }
//
//        [Fact]
//        public void CustomTitleIsAsConfigured()
//        {
//            var contextMock = new Mock<Context>();
//            const string title = "Custom title";
//            var runner = CreateScenario(contextMock.Object, title);
//
//            runner.Scenario.Name.Should().Be(title);
//        }
//
//        [Fact]
//        public void GivenSectionTextIsCorrect()
//        {
//            var contextMock = new Mock<Context>();
//            var runner = CreateScenario(contextMock.Object);
//            runner.Scenario.GivenSteps[0].Name.Should().Be("Given first fact");
//            runner.Scenario.GivenSteps[1].Name.Should().Be("And second fact");
//
//            // TODO: do testów DocFormatter
//            // runner.Scenario.Given.Should().Be($"Given first fact{Environment.NewLine}\tAnd second fact");
//        }

//        [Fact]
//        public void GivenLabelPrintedEvenWhenNoGivenBlocks()
//        {
//            var contextMock = new Mock<Context>();
//            var runner = Bdd.Scenario(contextMock.Object)
//                .When(c => c.SomethingIsDone())
//                .Then(c => c.Result1IsAsExpected())
//                .Create();
//
//            runner.Scenario.GivenSteps.Should().BeEmpty();
//            Given.Should().Be("Given no action");
//        }

//        [Fact]
//        public void WhenSectionTextIsCorrect()
//        {
//            var contextMock = new Mock<Context>();
//            var runner = CreateScenario(contextMock.Object);
//
//            runner.Scenario.WhenStep.Name.Should().Be("When something is done");
//        }
//
//        [Fact]
//        public void ThenSectionTextIsCorrect()
//        {
//            var contextMock = new Mock<Context>();
//            var runner = CreateScenario(contextMock.Object);
//
//            runner.Scenario.ThenSteps[0].Name.Should().Be("Then result 1 is as expected");
//            runner.Scenario.ThenSteps[1].Name.Should().Be("And result 2 is as expected");
//        }

//        [Fact]
//        public void DescriptionIsCorrectlyComposedFromSections()
//        {
//            var contextMock = new Mock<Context>();
//            var runner = CreateScenario(contextMock.Object);
//
//            var scenario = runner.Scenario;
//            var newLine = Environment.NewLine;
//            scenario.ToString().Should()
//                .Be($"Scenario: {scenario.Name}{newLine}{newLine}{scenario.Given}{newLine}{scenario.When}{newLine}{scenario.Then}{newLine}");
//        }

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

            void ExceptionIsThrown(WhenActionResult result);
        }
    }
}