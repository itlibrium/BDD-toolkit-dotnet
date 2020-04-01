using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ITLIBRIUM.BddToolkit.Syntax.Features;
using ITLIBRIUM.BddToolkit.Syntax.Rules;
using ITLIBRIUM.BddToolkit.Syntax.Scenarios;
using ITLIBRIUM.BddToolkit.Tests;
using Xunit;

namespace ITLIBRIUM.BddToolkit.Docs.Gherkin
{
    public class GherkinFilesPublisherTests
    {
        private readonly FakeFilesProvider _fakeFilesProvider;
        private readonly GherkinFilesPublisher _gherkinFilesPublisher;

        public GherkinFilesPublisherTests()
        {
            _fakeFilesProvider = new FakeFilesProvider();
            _gherkinFilesPublisher = new GherkinFilesPublisher(_fakeFilesProvider);
        }

        [Fact]
        public async Task FileIsCreatedPerEachFeature()
        {
            await Append(Scenario1, Scenario4, Scenario8);
            await Publish();

            FilesCountShouldBe(3);
        }

        [Fact]
        public async Task ScenariosAreGroupedPerRule()
        {
            await Append(Scenario1, Scenario2, Scenario3);
            await Publish();

            FileContentShouldBe(@"Feature: Feature 1

  Rule: Rule 1

    # Status: Passed
    Scenario: Scenario 1
      Given first fact
      When something is done
      Then result 1 is as expected

    # Status: Passed
    Scenario: Scenario 2
      Given first fact
      When something is done
      Then result 1 is as expected

  Rule: Rule 2

    # Status: Passed
    Scenario: Scenario 3
      Given first fact
      When something is done
      Then result 1 is as expected
");
        }

        [Fact]
        public async Task FreeScenariosArePlacedBeforeFirstRule()
        {
            await Append(Scenario4, Scenario5, Scenario6, Scenario7);
            await Publish();

            FileContentShouldBe(@"Feature: Feature 2

  # Status: Passed
  Scenario: Scenario 4
    Given first fact
    When something is done
    Then result 1 is as expected

  # Status: Passed
  Scenario: Scenario 5
    Given first fact
    When something is done
    Then result 1 is as expected

  Rule: Rule 3

    # Status: Passed
    Scenario: Scenario 6
      Given first fact
      When something is done
      Then result 1 is as expected

    # Status: Passed
    Scenario: Scenario 7
      Given first fact
      When something is done
      Then result 1 is as expected
");
        }

        [Fact]
        public async Task ScenariosWithoutFeatureAreGroupedInUnspecifiedFeature()
        {
            await Append(Scenario8, Scenario9);
            await Publish();

            FileContentShouldBe(@"Feature: Unspecified

  # Status: Passed
  Scenario: Scenario 8
    Given first fact
    When something is done
    Then result 1 is as expected

  Rule: Rule 4

    # Status: Passed
    Scenario: Scenario 9
      Given first fact
      When something is done
      Then result 1 is as expected
");
        }

        private async Task Append(params Scenario[] scenarios)
        {
            foreach (var scenario in scenarios)
                await _gherkinFilesPublisher.Append(scenario, TestStatus.Passed, CancellationToken.None);
        }

        private Task Publish() => _gherkinFilesPublisher.Publish(CancellationToken.None);

        private void FilesCountShouldBe(int count) => _fakeFilesProvider.StreamsContent.Count.Should().Be(count);

        private void FileContentShouldBe(string expectedFileContent)
        {
            _fakeFilesProvider.StreamsContent.Should().HaveCount(1);
            _fakeFilesProvider.StreamsContent[0].Should().Be(expectedFileContent);
        }

        private static Feature Feature1 { get; } = Bdd.Feature("Feature 1");

        private static Rule Rule1 { get; } = Bdd.Rule("Rule 1")
            .Feature(Feature1);

        private static Scenario Scenario1 { get; } = Bdd.Scenario<Context>()
            .Rule(Rule1)
            .Given(c => c.FirstFact())
            .When(c => c.SomethingIsDone())
            .Then(c => c.Result1IsAsExpected())
            .Create()
            .Scenario;

        private static Scenario Scenario2 { get; } = Bdd.Scenario<Context>()
            .Rule(Rule1)
            .Given(c => c.FirstFact())
            .When(c => c.SomethingIsDone())
            .Then(c => c.Result1IsAsExpected())
            .Create()
            .Scenario;

        private static Rule Rule2 { get; } = Bdd.Rule("Rule 2")
            .Feature(Feature1);

        private static Scenario Scenario3 { get; } = Bdd.Scenario<Context>()
            .Rule(Rule2)
            .Given(c => c.FirstFact())
            .When(c => c.SomethingIsDone())
            .Then(c => c.Result1IsAsExpected())
            .Create()
            .Scenario;

        private static Feature Feature2 { get; } = Bdd.Feature("Feature 2");

        private static Scenario Scenario4 { get; } = Bdd.Scenario<Context>()
            .Feature(Feature2)
            .Given(c => c.FirstFact())
            .When(c => c.SomethingIsDone())
            .Then(c => c.Result1IsAsExpected())
            .Create()
            .Scenario;

        private static Scenario Scenario5 { get; } = Bdd.Scenario<Context>()
            .Feature(Feature2)
            .Given(c => c.FirstFact())
            .When(c => c.SomethingIsDone())
            .Then(c => c.Result1IsAsExpected())
            .Create()
            .Scenario;

        private static Rule Rule3 { get; } = Bdd.Rule("Rule 3")
            .Feature(Feature2);

        private static Scenario Scenario6 { get; } = Bdd.Scenario<Context>()
            .Rule(Rule3)
            .Given(c => c.FirstFact())
            .When(c => c.SomethingIsDone())
            .Then(c => c.Result1IsAsExpected())
            .Create()
            .Scenario;

        private static Scenario Scenario7 { get; } = Bdd.Scenario<Context>()
            .Rule(Rule3)
            .Given(c => c.FirstFact())
            .When(c => c.SomethingIsDone())
            .Then(c => c.Result1IsAsExpected())
            .Create()
            .Scenario;

        private static Scenario Scenario8 { get; } = Bdd.Scenario<Context>()
            .Given(c => c.FirstFact())
            .When(c => c.SomethingIsDone())
            .Then(c => c.Result1IsAsExpected())
            .Create()
            .Scenario;

        private static Rule Rule4 { get; } = Bdd.Rule("Rule 4");

        private static Scenario Scenario9 { get; } = Bdd.Scenario<Context>()
            .Rule(Rule4)
            .Given(c => c.FirstFact())
            .When(c => c.SomethingIsDone())
            .Then(c => c.Result1IsAsExpected())
            .Create()
            .Scenario;

        [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Local")]
        private class Context
        {
            public void FirstFact() { }

            public void SomethingIsDone() { }

            public void Result1IsAsExpected() { }
        }
    }
}