using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ITLIBRIUM.BddToolkit.Syntax.Features;
using ITLIBRIUM.BddToolkit.Syntax.Rules;
using ITLIBRIUM.BddToolkit.Syntax.Scenarios;
using ITLIBRIUM.BddToolkit.Tests;
using Xunit;

namespace ITLIBRIUM.BddToolkit.Docs.Gherkin
{
    public class GherkinFileTests : IDisposable
    {
        private readonly Stream _stream = new MemoryStream();
        private GherkinFile _file;

        [Fact]
        public async Task EmptyFeatureWithoutDescriptionTest()
        {
            await CreateFileFor(FeatureWithoutDescription);
            
            FileContentShouldBe(@"Feature: Feature without description
");
        }

        [Fact]
        public async Task EmptyFeatureWithTagsTest()
        {
            await CreateFileFor(FeatureWithTags);
            
            FileContentShouldBe(@"@Tag-1 @tag-2
Feature: Feature with tags
");
        }

        [Fact]
        public async Task EmptyFeatureWithDescriptionTest()
        {
            await CreateFileFor(FeatureWithDescription);
            
            FileContentShouldBe(@"Feature: Feature With Description

  First line of feature description
  Second line of feature description
  Last line of feature description
");
        }

        [Fact]
        public async Task SingleScenarioWithTagsTest()
        {
            await CreateFileFor(FeatureWithoutDescription);
            await WriteToFile(SimpleScenarioWithTags, TestStatus.Unknown);
            
            FileContentShouldBe(@"Feature: Feature without description

  # Status: Unknown
  @tag-3 @Tag-4 @tag-5
  Scenario: Simple scenario with tags
    Given first fact
    When something is done
    Then result 1 is as expected
");
        }

        [Fact]
        public async Task SingleScenarioWithDescriptionTest()
        {
            await CreateFileFor(FeatureWithoutDescription);
            await WriteToFile(ComplexScenarioWithDescription, TestStatus.Unknown);
            
            FileContentShouldBe(@"Feature: Feature without description

  # Status: Unknown
  Scenario: Complex Scenario With Description

    First line of scenario description
    Second line of scenario description
    Last line of scenario description

    Given first fact
    And second fact
    And Custom Fact
    When something is done
    Then result 1 is as expected
    And result 2 is as expected
    And Custom Result
");
        }

        [Fact]
        public async Task MultipleScenariosTest()
        {
            await CreateFileFor(FeatureWithoutDescription);
            await WriteToFile(SimpleScenarioWithTags, TestStatus.Passed);
            await WriteToFile(ComplexScenarioWithDescription, TestStatus.Failed);
            
            FileContentShouldBe(@"Feature: Feature without description

  # Status: Passed
  @tag-3 @Tag-4 @tag-5
  Scenario: Simple scenario with tags
    Given first fact
    When something is done
    Then result 1 is as expected

  # Status: Failed
  Scenario: Complex Scenario With Description

    First line of scenario description
    Second line of scenario description
    Last line of scenario description

    Given first fact
    And second fact
    And Custom Fact
    When something is done
    Then result 1 is as expected
    And result 2 is as expected
    And Custom Result
");
        }

        [Fact]
        public async Task RuleWithoutDescriptionTest()
        {
            await CreateFileFor(FeatureWithoutDescription);
            await WriteToFile(RuleWithoutDescription);
            
            FileContentShouldBe(@"Feature: Feature without description

  Rule: Rule without description
");
        }

        [Fact]
        public async Task RuleWithDescriptionTest()
        {
            await CreateFileFor(FeatureWithoutDescription);
            await WriteToFile(RuleWithDescription);
            
            FileContentShouldBe(@"Feature: Feature without description

  Rule: Rule With Description

    First line of rule description
    Second line of rule description
    Last line of rule description
");
        }
        
        [Fact]
        public async Task AllCasesTest()
        {
            await CreateFileFor(FeatureWithDescription);
            await WriteToFile(SimpleScenarioWithTags, TestStatus.Unknown);
            await WriteToFile(ComplexScenarioWithDescription, TestStatus.Passed);
            await WriteToFile(RuleWithoutDescription);
            await WriteToFile(SimpleScenarioWithTags, TestStatus.Failed);
            await WriteToFile(ComplexScenarioWithDescription, TestStatus.Unknown);
            await WriteToFile(RuleWithDescription);
            await WriteToFile(SimpleScenarioWithTags, TestStatus.Passed);
            await WriteToFile(ComplexScenarioWithDescription, TestStatus.Failed);
            
            FileContentShouldBe(@"Feature: Feature With Description

  First line of feature description
  Second line of feature description
  Last line of feature description

  # Status: Unknown
  @tag-3 @Tag-4 @tag-5
  Scenario: Simple scenario with tags
    Given first fact
    When something is done
    Then result 1 is as expected

  # Status: Passed
  Scenario: Complex Scenario With Description

    First line of scenario description
    Second line of scenario description
    Last line of scenario description

    Given first fact
    And second fact
    And Custom Fact
    When something is done
    Then result 1 is as expected
    And result 2 is as expected
    And Custom Result

  Rule: Rule without description

    # Status: Failed
    @tag-3 @Tag-4 @tag-5
    Scenario: Simple scenario with tags
      Given first fact
      When something is done
      Then result 1 is as expected

    # Status: Unknown
    Scenario: Complex Scenario With Description

      First line of scenario description
      Second line of scenario description
      Last line of scenario description

      Given first fact
      And second fact
      And Custom Fact
      When something is done
      Then result 1 is as expected
      And result 2 is as expected
      And Custom Result

  Rule: Rule With Description

    First line of rule description
    Second line of rule description
    Last line of rule description

    # Status: Passed
    @tag-3 @Tag-4 @tag-5
    Scenario: Simple scenario with tags
      Given first fact
      When something is done
      Then result 1 is as expected

    # Status: Failed
    Scenario: Complex Scenario With Description

      First line of scenario description
      Second line of scenario description
      Last line of scenario description

      Given first fact
      And second fact
      And Custom Fact
      When something is done
      Then result 1 is as expected
      And result 2 is as expected
      And Custom Result
");
        }

        private async Task CreateFileFor(Feature feature) => _file = await GherkinFile.For(feature, _stream);

        private Task WriteToFile(Rule rule) => _file.Write(rule);

        private Task WriteToFile(Scenario scenario, TestStatus testStatus) => _file.Write(scenario, testStatus);

        private void FileContentShouldBe(string value)
        {
            _file.Dispose();
            _stream.Position = 0;
            using var reader = new StreamReader(_stream, Encoding.UTF8, leaveOpen: true);
            reader.ReadToEnd().Should().Be(value);
        }
        
        private static Feature FeatureWithoutDescription { get; } = Bdd.Feature("Feature without description");

        private static Feature FeatureWithDescription { get; } = Bdd.Feature("Feature With Description")
            .Description(@"First line of feature description
Second line of feature description
Last line of feature description");

        private static Feature FeatureWithTags { get; } = Bdd.Feature("Feature with tags")
            .Tags("Tag-1", "tag-2");

        private static Rule RuleWithoutDescription { get; } = Bdd.Rule("Rule without description");

        private static Rule RuleWithDescription { get; } = Bdd.Rule("Rule With Description")
            .Description(@"First line of rule description
Second line of rule description
Last line of rule description");

        private static Scenario SimpleScenarioWithTags { get; } = Bdd.Scenario<Context>()
            .Tags("tag-3", "Tag-4", "tag-5")
            .Given(c => c.FirstFact())
            .When(c => c.SomethingIsDone())
            .Then(c => c.Result1IsAsExpected())
            .Create()
            .Scenario;

        private static Scenario ComplexScenarioWithDescription { get; } = Bdd.Scenario<Context>()
            .Name("Complex Scenario With Description")
            .Description(@"First line of scenario description
Second line of scenario description
Last line of scenario description")
            .Given(c => c.FirstFact())
            .And(c => c.SecondFact())
            .And(c => c.ThirdFact(), "Custom Fact")
            .When(c => c.SomethingIsDone())
            .Then(c => c.Result1IsAsExpected())
            .And(c => c.Result2IsAsExpected())
            .And(c => c.Result3IsAsExpected(), "Custom Result")
            .Create()
            .Scenario;

        [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Local")]
        private class Context
        {
            public void FirstFact() { }
            public void SecondFact() { }
            
            public void ThirdFact() { }

            public void SomethingIsDone() { }

            public void Result1IsAsExpected() { }
            public void Result2IsAsExpected() { }
            
            public void Result3IsAsExpected() { }
        }
        
        public void Dispose() => _stream.Dispose();
    }
}