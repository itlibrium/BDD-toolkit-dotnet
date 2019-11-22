namespace ITLIBRIUM.BddToolkit.Docs.Gherkin
{
    public class GherkinFormatterTests
    {
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
    }
}