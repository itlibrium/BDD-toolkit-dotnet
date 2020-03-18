using System;
using System.Threading;
using ITLIBRIUM.BddToolkit.Docs;
using ITLIBRIUM.BddToolkit.Syntax.Scenarios;
using ITLIBRIUM.BddToolkit.Tests.Results;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Execution
{
    public readonly struct TestedScenario : IEquatable<TestedScenario>
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
        public void ThrowOnErrors() => TestResult.ThrowOnErrors();

        public bool Equals(TestedScenario other) => (Scenario, TestResult).Equals((other.Scenario, other.TestResult));
        public override bool Equals(object obj) => obj is TestedScenario other && Equals(other);
        public override int GetHashCode() => (Scenario, TestResult).GetHashCode();
    }
}