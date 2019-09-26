//using System;
//using ITLIBRIUM.BddToolkit.Syntax.Scenarios;
//
//namespace ITLIBRIUM.BddToolkit.Tests
//{
//    public readonly struct ScenarioDescriptor : IEquatable<ScenarioDescriptor>
//    {
//        public Scenario Scenario { get; }
//        public TestStatus TestStatus { get; }
//        
//        public static ScenarioDescriptor AfterTest(Scenario scenario, bool isTestSuccessful) =>
//            new ScenarioDescriptor(scenario, isTestSuccessful ? TestStatus.Passed : TestStatus.Failed);
//        
//        public static ScenarioDescriptor WithoutTest(Scenario scenario) =>
//            new ScenarioDescriptor(scenario, TestStatus.Unknown);
//        
//        private ScenarioDescriptor(Scenario scenario, TestStatus testStatus)
//        {
//            Scenario = scenario;
//            TestStatus = testStatus;
//        }
//
//        public bool Equals(ScenarioDescriptor other) =>
//            (Description: Scenario, TestStatus).Equals((other.Scenario, other.TestStatus));
//        public override bool Equals(object obj) => obj is ScenarioDescriptor other && Equals(other);
//        public override int GetHashCode() => (Description: Scenario, TestStatus).GetHashCode();
//    }
//}