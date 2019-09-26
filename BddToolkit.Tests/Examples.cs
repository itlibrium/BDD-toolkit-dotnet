//using ITLIBRIUM.BddToolkit.Features;
//using ITLIBRIUM.BddToolkit.Rules;
//using Xunit;
//
//namespace ITLIBRIUM.BddToolkit.Tests
//{
//    public class Examples
//    {
//        public static readonly Feature Feature1 = Bdd.Feature("Feature 1");
//        public static readonly Rule Rule1 = Bdd.Rule(Feature1, "Rule 1");
//
//        [Fact]
//        public void Scenario1() => Bdd.Scenario<Context>()
//            .Feature(Feature1)
//            .Rule(Rule1)
//            .Name("name of scenario 1")
//            .Description("description for scenario 1")
//            .TestedComponent("some component")
//            .Given(c => c.Sth())
//            .And(c => c.Sth())
//            .When(c => c.Do())
//            .Then(c => c.Done())
//            .And(c => c.Done())
//            .Test();
//        
//        private class Context
//        {
//            public void Sth()
//            {
//                throw new System.NotImplementedException();
//            }
//
//            public void Do()
//            {
//                throw new System.NotImplementedException();
//            }
//
//            public void Done()
//            {
//                throw new System.NotImplementedException();
//            }
//        }
//    }
//
//    [Feature("Feature1", "Description od feature1")]
//    public class Feature1
//    {
//        [Rule("Rule1", "Description of rule1")]
//        public class Rule1
//        {
//            //[Fact]
//            [Scenario("")]
//            [TestedComponent("")]
//            public void Scenario11() => Bdd.Scenario<Context>()
//                .Given(c => c.Sth())
//                .And(c => c.Sth())
//                .When(c => c.Do())
//                .Then(c => c.Done())
//                .And(c => c.Done())
//                .Test();
//            
////            [Fact]
//            [Scenario("")]
//            [TestedComponent("")]
//            public void Scenario12() => Bdd.Scenario<Context>()
//                .Given(c => c.Sth())
//                .And(c => c.Sth())
//                .When(c => c.Do())
//                .Then(c => c.Done())
//                .And(c => c.Done())
//                .Test();
//        }
//        
//        [Rule("Rule2", "Description of rule2")]
//        public class Rule2
//        {
////            [Fact]
//            [Scenario("")]
//            [TestedComponent("")]
//            public void Scenario21() => Bdd.Scenario<Context>()
//                .Given(c => c.Sth())
//                .And(c => c.Sth())
//                .When(c => c.Do())
//                .Then(c => c.Done())
//                .And(c => c.Done())
//                .Test();
//            
////            [Fact]
//            [Scenario("")]
//            [TestedComponent("")]
//            public void Scenario22() => Bdd.Scenario<Context>()
//                .Given(c => c.Sth())
//                .And(c => c.Sth())
//                .When(c => c.Do())
//                .Then(c => c.Done())
//                .And(c => c.Done())
//                .Test();
//        }
//        
//        private class Context
//        {
//            public void Sth()
//            {
//                throw new System.NotImplementedException();
//            }
//
//            public void Do()
//            {
//                throw new System.NotImplementedException();
//            }
//
//            public void Done()
//            {
//                throw new System.NotImplementedException();
//            }
//        }
//    }
//}