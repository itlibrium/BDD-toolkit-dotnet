using System;
using System.Threading;
using System.Threading.Tasks;
using ITLIBRIUM.BddToolkit.Syntax.Scenarios;
using ITLIBRIUM.BddToolkit.Tests;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Docs
{
    public class DocPublisher : IDisposable
    {
        private readonly DocFormatter _formatter;
        private readonly DocsWriter _writer;
        
        public static DocPublisher Gherkin => new DocPublisher(
            new GherkinDocsFormatter(),
            new GherkinDocFileWriter());

        public DocPublisher([NotNull] DocFormatter formatter, [NotNull] DocsWriter writer)
        {
            _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        public Task Publish(Scenario scenario, TestStatus testStatus) =>
            Publish(scenario, testStatus, CancellationToken.None);

        public Task Publish(Scenario scenario, TestStatus testStatus, CancellationToken cancellationToken) => 
            _formatter.Write(scenario, testStatus, _writer, cancellationToken);

        public void Dispose()
        {
            _formatter.Dispose();
            _writer.Dispose();
        }
    }
}