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

        public TestedScenario PublishDoc(DocPublisher docPublisher)
        {
            docPublisher.Publish(Scenario, TestResult.IsSuccessful ? TestStatus.Passed : TestStatus.Failed);
            return this;
        }

        public void ThrowOnErrors()
        {
            if (!TestResult.IsSuccessful) throw new AggregateAssertException(TestResult.Exceptions);
        }
    }
}