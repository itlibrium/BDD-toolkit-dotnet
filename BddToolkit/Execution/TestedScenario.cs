using System.Threading;
using ITLIBRIUM.BddToolkit.Docs;
using ITLIBRIUM.BddToolkit.Syntax.Scenarios;
using ITLIBRIUM.BddToolkit.Tests;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Execution
{
    public readonly struct TestedScenario
    {
        [PublicAPI]
        public Scenario Scenario { get; }
        
        [PublicAPI]
        public TestResult TestResult { get; }

        public TestedScenario(Scenario scenario, TestResult testResult)
        {
            Scenario = scenario;
            TestResult = testResult;
        }

        [PublicAPI]
        public TestedScenario PublishDoc(DocPublisher docPublisher, CancellationToken cancellationToken)
        {
            docPublisher.Append(Scenario, TestResult.ToTestStatus(), cancellationToken);
            return this;
        }

        [PublicAPI]
        public void ThrowOnErrors()
        {
            if (!TestResult.IsSuccessful) throw new AggregateAssertException(TestResult.Exceptions);
        }
    }
}