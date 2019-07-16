using System;
using FluentAssertions;
using ITLibrium.Bdd.Scenarios;
using Moq;
using Xunit;

namespace ITLibrium.BDD.Tests.Scenarios
{
    public class BddScenarioTests
    {
        [Fact]
        public void AllGivenActionsAreInvokedOnce()
        {
            var fixtureMock = new Mock<IFixture>();

            TestScenario(fixtureMock.Object);

            fixtureMock.Verify(f => f.FirstFact(), Times.Once);
            fixtureMock.Verify(f => f.SecondFact(), Times.Once);
        }

        [Fact]
        public void GivenActionsAreNotRequired()
        {
            var fixtureMock = new Mock<IFixture>();
            Action test = () => BddScenario
                .ExcludeFromReports()
                .Using(fixtureMock.Object)
                .When(f => f.SomethingIsDone())
                .Then(f => f.Result1IsAsExpected())
                .Test();
            test.Should().NotThrow();
        }

        [Fact]
        public void WhenActionIsInvokedOnce()
        {
            var fixtureMock = new Mock<IFixture>();

            TestScenario(fixtureMock.Object);

            fixtureMock.Verify(f => f.SomethingIsDone(), Times.Once);
        }

        [Fact]
        public void CreateSutIsInvokedOnceForISutCreator()
        {
            var fixtureMock = new Mock<IFixture>();

            TestScenario(fixtureMock.Object);

            fixtureMock.Verify(f => f.CreateSut(), Times.Once);
        }

        [Fact]
        public void AllThenActionsAreInvokedOnce()
        {
            var fixtureMock = new Mock<IFixture>();

            TestScenario(fixtureMock.Object);

            fixtureMock.Verify(f => f.Result1IsAsExpected(), Times.Once);
            fixtureMock.Verify(f => f.Result2IsAsExpected(), Times.Once);
        }

        [Fact]
        public void AllThenActionsAreInvokedEvenIfExceptionWasThrown()
        {
            var fixtureMock = new Mock<IFixture>();
            fixtureMock.Setup(f => f.SomethingIsDone()).Throws<Exception>();

            Action test = () => TestScenario(fixtureMock.Object);

            test.Should().Throw<AggregateAssertException>();
            fixtureMock.Verify(f => f.Result1IsAsExpected(), Times.Once);
            fixtureMock.Verify(f => f.Result2IsAsExpected(), Times.Once);
        }

        [Fact]
        public void SecondThenActionIsInvokedEvenIfFirstAssertFailed()
        {
            var fixtureMock = new Mock<IFixture>();
            fixtureMock.Setup(f => f.Result1IsAsExpected()).Throws<Exception>();

            Action test = () => TestScenario(fixtureMock.Object);

            test.Should().Throw<AggregateAssertException>();
            fixtureMock.Verify(f => f.Result1IsAsExpected(), Times.Once);
            fixtureMock.Verify(f => f.Result2IsAsExpected(), Times.Once);
        }

        [Fact]
        public void TestPassesWhenExceptionIsThrownAndExplicitAssertionIsMade()
        {
            var fixtureMock = new Mock<IFixture>();
            fixtureMock.Setup(f => f.SomethingIsDone()).Throws<Exception>();

            Action test = () => BddScenario
                .ExcludeFromReports()
                .Using(fixtureMock.Object)
                .When(f => f.SomethingIsDone())
                .Then((f, e) => f.ExceptionIsThrown(e))
                .Test();
            test.Should().NotThrow();

            Action test2 = () => BddScenario
                .ExcludeFromReports()
                .Using(fixtureMock.Object)
                .When(f => f.SomethingIsDone())
                .Then(f => f.Result1IsAsExpected())
                .And((f, e) => f.ExceptionIsThrown(e))
                .Test();
            test2.Should().NotThrow();
        }

        [Fact]
        public void TestFailsWhenExceptionIsThrownAndNoExplicitAssertionIsMade()
        {
            var fixtureMock = new Mock<IFixture>();
            fixtureMock.Setup(f => f.SomethingIsDone()).Throws<Exception>();

            Action test = () => BddScenario
                .ExcludeFromReports()
                .Using(fixtureMock.Object)
                .Given(f => f.FirstFact())
                .When(f => f.SomethingIsDone())
                .Then(f => f.Result1IsAsExpected())
                .Test();

            test.Should().Throw<AggregateAssertException>();
        }

        [Fact]
        public void DefaultTitleIsEqualToHumanizedMethodName()
        {
            var fixtureMock = new Mock<IFixture>();
            var scenario = BddScenario
                .ExcludeFromReports()
                .Using(fixtureMock.Object)
                .Given(f => f.FirstFact())
                .And(f => f.SecondFact())
                .When(f => f.SomethingIsDone())
                .Then(f => f.Result1IsAsExpected())
                .And(f => f.Result2IsAsExpected())
                .Create();

            scenario.GetDescription().Title.Should().Be("Default title is equal to humanized method name");
        }

        [Fact]
        public void CustomTitleIsAsConfigured()
        {
            var fixtureMock = new Mock<IFixture>();
            const string title = "Custom title";
            var scenario = CreateScenario(fixtureMock.Object, title);

            scenario.GetDescription().Title.Should().Be(title);
        }

        [Fact]
        public void GivenSectionTextIsCorrect()
        {
            var fixtureMock = new Mock<IFixture>();
            var scenario = CreateScenario(fixtureMock.Object);

            scenario.GetDescription().Given.Should().Be($"Given first fact{Environment.NewLine}\tAnd second fact");
        }

        [Fact]
        public void GivenLabelPrintedEvenWhenNoGivenBlocks()
        {
            var fixtureMock = new Mock<IFixture>();
            var scenario = BddScenario
                .Using(fixtureMock.Object)
                .When(f => f.SomethingIsDone())
                .Then(f => f.Result1IsAsExpected())
                .Create();

            scenario.GetDescription().Given.Should().Be("Given no action");
        }

        [Fact]
        public void WhenSectionTextIsCorrect()
        {
            var fixtureMock = new Mock<IFixture>();
            var scenario = CreateScenario(fixtureMock.Object);

            scenario.GetDescription().When.Should().Be("When something is done");
        }

        [Fact]
        public void ThenSectionTextIsCorrect()
        {
            var fixtureMock = new Mock<IFixture>();
            var scenario = CreateScenario(fixtureMock.Object);

            scenario.GetDescription().Then.Should()
                .Be($"Then result 1 is as expected{Environment.NewLine}\tAnd result 2 is as expected");
        }

        [Fact]
        public void DescriptionIsCorrectlyComposedFromSections()
        {
            var fixtureMock = new Mock<IFixture>();
            var scenario = CreateScenario(fixtureMock.Object);

            var description = scenario.GetDescription();
            var newLine = Environment.NewLine;
            description.ToString().Should()
                .Be($"Scenario: {description.Title}{newLine}{newLine}{description.Given}{newLine}{description.When}{newLine}{description.Then}{newLine}");
        }

        private static void TestScenario(IFixture fixture)
        {
            CreateScenario(fixture).Test();
        }

        private static IBddScenario CreateScenario(IFixture fixture, string title = null)
        {
            return BddScenario
                .Title(title)
                .ExcludeFromReports()
                .Using(fixture)
                .Given(f => f.FirstFact())
                .And(f => f.SecondFact())
                .When(f => f.SomethingIsDone())
                .Then(f => f.Result1IsAsExpected())
                .And(f => f.Result2IsAsExpected())
                .Create();
        }

        // ReSharper disable once MemberCanBePrivate.Global - required by Moq
        public interface IFixture : ISutCreator
        {
            void FirstFact();
            void SecondFact();

            void SomethingIsDone();

            void Result1IsAsExpected();
            void Result2IsAsExpected();

            void ExceptionIsThrown(Exception exception);
        }
    }
}