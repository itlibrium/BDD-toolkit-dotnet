using System;
using System.Threading;
using ITLIBRIUM.BddToolkit.Docs;
using ITLIBRIUM.BddToolkit.Syntax.Scenarios;
using ITLIBRIUM.BddToolkit.Tests;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Execution
{
    public readonly struct TestableScenario : IEquatable<TestableScenario>
    {
        [PublicAPI]
        public Scenario Scenario { get; }
        
        [PublicAPI]
        public ScenarioTest Test { get; }

        public TestableScenario(Scenario scenario, [NotNull] ScenarioTest test)
        {
            Scenario = scenario;
            Test = test ?? throw new ArgumentNullException(nameof(test));
        }

        [PublicAPI]
        public TestedScenario RunTest()
        {
            var testResult = Test.Run();
            return new TestedScenario(Scenario, testResult);
        }
        
        [PublicAPI]
        public TestableScenario PublishDoc(DocPublisher docPublisher, CancellationToken cancellationToken)
        {
            docPublisher.Append(Scenario, TestStatus.Unknown, cancellationToken);
            return this;
        }

        public bool Equals(TestableScenario other) => (Scenario, Test).Equals((other.Scenario, other.Test));
        public override bool Equals(object obj) => obj is TestableScenario other && Equals(other);
        public override int GetHashCode() => (Scenario, Test).GetHashCode();
    }
}