using System.Threading;
using ITLIBRIUM.BddToolkit.Docs;
using ITLIBRIUM.BddToolkit.Syntax.Scenarios;
using ITLIBRIUM.BddToolkit.Tests;

namespace ITLIBRIUM.BddToolkit.Execution
{
    public readonly struct TestedScenario
    {
        public Scenario Scenario { get; }
        public TestResult TestResult { get; }

        public TestedScenario(Scenario scenario, TestResult testResult)
        {
            Scenario = scenario;
            TestResult = testResult;
        }

        public TestedScenario PublishDoc(DocPublisher docPublisher, CancellationToken cancellationToken)
        {
            docPublisher.Append(Scenario, TestResult.ToTestStatus(), cancellationToken);
            return this;
        }

        public void ThrowOnErrors()
        {
            if (!TestResult.IsSuccessful) throw new AggregateAssertException(TestResult.Exceptions);
        }
    }
}