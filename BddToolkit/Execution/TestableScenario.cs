using System.Threading;
using ITLIBRIUM.BddToolkit.Docs;
using ITLIBRIUM.BddToolkit.Syntax.Scenarios;
using ITLIBRIUM.BddToolkit.Tests;

namespace ITLIBRIUM.BddToolkit.Execution
{
    public readonly struct TestableScenario
    {
        private readonly Scenario _scenario;
        private readonly ScenarioTest _test;

        public TestableScenario(Scenario scenario, ScenarioTest test)
        {
            _scenario = scenario;
            _test = test;
        }

        public TestedScenario RunTest()
        {
            var testResult = _test.Run();
            return new TestedScenario(_scenario, testResult);
        }
        
        public TestableScenario PublishDoc(DocPublisher docPublisher, CancellationToken cancellationToken)
        {
            docPublisher.Append(_scenario, TestStatus.Unknown, cancellationToken);
            return this;
        }
    }
}