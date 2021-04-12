using System;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ITLIBRIUM.BddToolkit.Syntax.Features;
using ITLIBRIUM.BddToolkit.Syntax.Rules;
using ITLIBRIUM.BddToolkit.Syntax.Scenarios;
using ITLIBRIUM.BddToolkit.Tests;

namespace ITLIBRIUM.BddToolkit.Docs.Gherkin
{
    internal class GherkinFile : IDisposable
    {
        private const string Feature = "Feature:";
        private const string Rule = "Rule:";
        private const string Scenario = "Scenario:";
        private const string Given = "Given";
        private const string When = "When";
        private const string Then = "Then";
        private const string And = "And";
        private const string Comment = "#";

        private readonly StreamWriter _writer;

        private Entry _lastEntry = Entry.Non;

        public static async Task<GherkinFile> For(Feature feature, Stream stream)
        {
            var file = new GherkinFile(stream);
            await file.Write(feature);
            return file;
        }

        private GherkinFile(Stream stream) =>
            _writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 4 * 1024, leaveOpen: true);

        private async Task Write(Feature feature)
        {
            await WriteTags(feature.Tags, 0);
            await WriteKeyword(Feature, feature.IsEmpty ? "Unspecified" : feature.Name, 0);
            if (!string.IsNullOrWhiteSpace(feature.Description))
            {
                await WriteEmptyLine();
                await WriteDescription(feature.Description, 1);
            }
            _lastEntry = Entry.Feature;
        }

        public async Task Write(Rule rule)
        {
            await WriteEmptyLine();
            await WriteKeyword(Rule, rule.Name, 1);
            if (!string.IsNullOrWhiteSpace(rule.Description))
            {
                await WriteEmptyLine();
                await WriteDescription(rule.Description, 2);
            }
            _lastEntry = Entry.Rule;
        }

        public async Task Write(Scenario scenario, TestStatus testStatus)
        {
            var indentationLevel = _lastEntry switch
            {
                Entry.Feature => 1,
                Entry.Rule => 2,
                _ => throw new ArgumentOutOfRangeException(nameof(_lastEntry))
            };
            await WriteEmptyLine();
            await WriteKeyword(Comment, $"Status: {testStatus}", indentationLevel);
            await WriteTags(scenario.Tags, indentationLevel);
            await WriteKeyword(Scenario, scenario.Name, indentationLevel);
            if (!string.IsNullOrWhiteSpace(scenario.Description))
            {
                await WriteEmptyLine();
                await WriteDescription(scenario.Description, indentationLevel + 1);
                await WriteEmptyLine();
            }
            await WriteSteps(Given, scenario.GivenSteps, indentationLevel + 1);
            if (scenario.WhenStep.HasValue)
                await WriteStep(When, scenario.WhenStep.Value, indentationLevel + 1);
            await WriteSteps(Then, scenario.ThenSteps, indentationLevel + 1);
        }

        private async Task WriteTags(ImmutableArray<string> tags, int indentationLevel)
        {
            var count = tags.Length;
            if (count == 0)
                return;

            await _writer.WriteAsync(Indentation(indentationLevel));
            await _writer.WriteAsync('@');
            await _writer.WriteAsync(tags[0]);
            for (var i = 1; i < count; i++)
            {
                await _writer.WriteAsync(' ');
                await _writer.WriteAsync('@');
                await _writer.WriteAsync(tags[i]);
            }
            await _writer.WriteLineAsync();
        }

        private async Task WriteKeyword(string keyword, string value, int indentationLevel)
        {
            await _writer.WriteAsync(Indentation(indentationLevel));
            await _writer.WriteAsync(keyword);
            await _writer.WriteAsync(' ');
            await _writer.WriteLineAsync(value);
        }

        private async Task WriteDescription(string description, int indentationLevel)
        {
            foreach (var line in description.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries))
            {
                await _writer.WriteAsync(Indentation(indentationLevel));
                await _writer.WriteLineAsync(line);
            }
        }

        private async Task WriteSteps<TStep>(string keyword, ImmutableArray<TStep> steps, int indentationLevel)
            where TStep : struct, ScenarioStep
        {
            var count = steps.Length;
            if (count == 0)
                return;

            await WriteStep(keyword, steps[0], indentationLevel);
            for (var i = 1; i < steps.Length; i++)
                await WriteStep(And, steps[i], indentationLevel);
        }

        private async Task WriteStep<TStep>(string keyword, TStep step, int indentationLevel)
            where TStep : struct, ScenarioStep
        {
            await _writer.WriteAsync(Indentation(indentationLevel));
            await _writer.WriteLineAsync($"{keyword} {step.Name}");
        }

        private Task WriteEmptyLine() => _writer.WriteLineAsync();

        private static string Indentation(int level) => new string(' ', level * 2);

        public Task Flush() => _writer.FlushAsync();

        public void Dispose() => _writer.Dispose();

        private enum Entry
        {
            Non,
            Feature,
            Rule,
        }
    }
}