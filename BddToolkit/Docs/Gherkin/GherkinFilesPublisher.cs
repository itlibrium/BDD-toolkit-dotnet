using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ITLIBRIUM.BddToolkit.Syntax.Features;
using ITLIBRIUM.BddToolkit.Syntax.Rules;
using ITLIBRIUM.BddToolkit.Syntax.Scenarios;
using ITLIBRIUM.BddToolkit.Tests;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Docs.Gherkin
{
    public class GherkinFilesPublisher : DocPublisher
    {
        private readonly string _basePath;
        private readonly FilesProvider _filesProvider;

        private readonly Dictionary<Feature, Scenarios> _scenariosPerFeature = new Dictionary<Feature, Scenarios>();

        private readonly Scenarios _scenariosWithoutFeature = new Scenarios();

        public GherkinFilesPublisher([NotNull] FilesProvider filesProvider)
        {
            _basePath = Path.Combine(AppContext.BaseDirectory, "TestScenarios");
            _filesProvider = filesProvider ?? throw new ArgumentNullException(nameof(filesProvider));
        }

        public GherkinFilesPublisher([NotNull] string basePath, [NotNull] FilesProvider filesProvider)
        {
            if (string.IsNullOrWhiteSpace(basePath))
                throw new ArgumentException($"{nameof(basePath)} can not be empty", nameof(basePath));
            _basePath = basePath;
            _filesProvider = filesProvider ?? throw new ArgumentNullException(nameof(filesProvider));
        }

        public Task Append(in Scenario scenario, TestStatus testStatus, CancellationToken cancellationToken)
        {
            if (scenario.Feature.IsEmpty)
            {
                _scenariosWithoutFeature.Add(scenario, testStatus);
            }
            else
            {
                if (!_scenariosPerFeature.TryGetValue(scenario.Feature, out var scenarios))
                    _scenariosPerFeature.Add(scenario.Feature, scenarios = new Scenarios());
                scenarios.Add(scenario, testStatus);
            }
            return Task.CompletedTask;
        }

        public async Task Publish(CancellationToken cancellationToken)
        {
            await _filesProvider.CreateDirectory(_basePath);
            await Task.WhenAll(WriteAllFeatureFiles());
        }

        private IEnumerable<Task> WriteAllFeatureFiles()
        {
            foreach (var pair in _scenariosPerFeature)
            {
                var feature = pair.Key;
                var scenarios = pair.Value;
                yield return WriteFeatureFile(feature, scenarios);
            }
            if (_scenariosWithoutFeature.Count > 0)
                yield return WriteFeatureFile(Feature.Empty(), _scenariosWithoutFeature);
        }

        private async Task WriteFeatureFile(Feature feature, Scenarios scenarios)
        {
            var path = Path.Combine(_basePath, feature.IsEmpty ? "Unspecified.feature" : $"{feature.Name}.feature");
            using var stream = await _filesProvider.CreateFile(path);
            using var file = await GherkinFile.For(feature, stream);
            await scenarios.WriteTo(file);
        }

        public void Dispose()
        {
            // nothing to dispose
        }

        private class Scenarios
        {
            private readonly Dictionary<Rule, List<(Scenario, TestStatus)>> _scenariosPerRule =
                new Dictionary<Rule, List<(Scenario, TestStatus)>>();

            private readonly List<(Scenario, TestStatus)> _scenariosWithoutRule = new List<(Scenario, TestStatus)>();

            public int Count => _scenariosPerRule.Count + _scenariosWithoutRule.Count;

            public void Add(in Scenario scenario, TestStatus testStatus)
            {
                if (scenario.Rule.IsEmpty)
                {
                    _scenariosWithoutRule.Add((scenario, testStatus));
                }
                else
                {
                    if (!_scenariosPerRule.TryGetValue(scenario.Rule, out var scenarios))
                        _scenariosPerRule.Add(scenario.Rule, scenarios = new List<(Scenario, TestStatus)>());
                    scenarios.Add((scenario, testStatus));
                }
            }

            public async Task WriteTo(GherkinFile file)
            {
                foreach (var (scenario, testStatus) in _scenariosWithoutRule)
                    await file.Write(scenario, testStatus);

                foreach (var pair in _scenariosPerRule)
                {
                    var rule = pair.Key;
                    var scenarios = pair.Value;
                    await file.Write(rule);
                    foreach (var (scenario, testStatus) in scenarios)
                        await file.Write(scenario, testStatus);
                }
            }
        }
    }
}