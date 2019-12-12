using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using ITLIBRIUM.BddToolkit.Docs;
using ITLIBRIUM.BddToolkit.Execution;
using ITLIBRIUM.BddToolkit.Syntax.Features;
using ITLIBRIUM.BddToolkit.Syntax.Rules;
using ITLIBRIUM.BddToolkit.Syntax.Scenarios;
using ITLIBRIUM.BddToolkit.Tests;
using ITLibrium.Reflection;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Builders
{
    internal class ScenarioBuilder<TContext> :
        IScenarioDescriptionBuilder<TContext>,
        IGivenContinuationBuilder<TContext>,
        IThenBuilder<TContext>,
        IThenContinuationBuilder<TContext>
        where TContext : class
    {
        private readonly TContext _context;
        private readonly DocPublisher _docPublisher;

        private Feature _feature;
        private Rule _rule;
        private string _name;
        private string _description;
        private readonly List<GivenStep> _givenSteps = new List<GivenStep>();
        private readonly List<GivenAction<TContext>> _givenActions = new List<GivenAction<TContext>>();
        private WhenStep _whenStep;
        private WhenAction<TContext> _whenAction;
        private readonly List<ThenStep> _thenSteps = new List<ThenStep>();
        private readonly List<ThenAction<TContext>> _thenActions = new List<ThenAction<TContext>>();
        private readonly List<ExceptionCheck<TContext>> _exceptionChecks = new List<ExceptionCheck<TContext>>();
        
        private bool _isCompleted;

        internal ScenarioBuilder([NotNull] TContext context, [NotNull] DocPublisher docPublisher)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _docPublisher = docPublisher ?? throw new ArgumentNullException(nameof(docPublisher));
        }

        ~ScenarioBuilder()
        {
            if (!_isCompleted)
                throw new InvalidOperationException(
                    $"{nameof(ScenarioBuilder<TContext>)} should create scenario. Call Create or Test method after Then section");
        }

        public IScenarioDescriptionBuilder<TContext> Feature(Feature feature)
        {
            _feature = feature;
            return this;
        }

        public IScenarioDescriptionBuilder<TContext> Rule(Rule rule)
        {
            _feature = rule.Feature;
            _rule = rule;
            return this;
        }

        public IScenarioDescriptionBuilder<TContext> Name(string name)
        {
            _name = name;
            return this;
        }

        public IScenarioDescriptionBuilder<TContext> Description(string description)
        {
            _description = description;
            return this;
        }

        public IGivenContinuationBuilder<TContext> Given(Expression<Action<TContext>> action) => 
            Given(action.Compile(), action.GetName());

        public IGivenContinuationBuilder<TContext> Given(Action<TContext> action, string name)
        {
            _givenSteps.Add(new GivenStep(name));
            _givenActions.Add(new GivenAction<TContext>(action));
            return this;
        }

        IGivenContinuationBuilder<TContext> IGivenContinuationBuilder<TContext>.And(
            Expression<Action<TContext>> action) =>
            Given(action);

        IGivenContinuationBuilder<TContext> IGivenContinuationBuilder<TContext>.And(
            Action<TContext> action, string name) =>
            Given(action, name);

        public IThenBuilder<TContext> When(Expression<Action<TContext>> action) => 
            When(action.Compile(), action.GetName());

        public IThenBuilder<TContext> When(Action<TContext> action, string name)
        {
            _whenStep = new WhenStep(name);
            _whenAction = new WhenAction<TContext>(action);
            return this;
        }

        public IThenContinuationBuilder<TContext> Then(Expression<Action<TContext>> action) => 
            Then(action.Compile(), action.GetName());

        public IThenContinuationBuilder<TContext> Then(Action<TContext> thenAction, string name)
        {
            _thenSteps.Add(new ThenStep(name));
            _thenActions.Add(new ThenAction<TContext>(thenAction));
            return this;
        }

        public IThenContinuationBuilder<TContext> Then(Expression<Action<TContext, Result>> exceptionCheck) => 
            Then(exceptionCheck.Compile(), exceptionCheck.GetName());

        public IThenContinuationBuilder<TContext> Then(Action<TContext, Result> exceptionCheck, string name)
        {
            _thenSteps.Add(new ThenStep(name));
            _exceptionChecks.Add(new ExceptionCheck<TContext>(exceptionCheck));
            return this;
        }

        IThenContinuationBuilder<TContext> IThenContinuationBuilder<TContext>.And(
            Expression<Action<TContext>> thenAction) =>
            Then(thenAction);

        IThenContinuationBuilder<TContext> IThenContinuationBuilder<TContext>.And(
            Action<TContext> thenAction, string name) =>
            Then(thenAction, name);

        IThenContinuationBuilder<TContext> IThenContinuationBuilder<TContext>.And(
            Expression<Action<TContext, Result>> exceptionCheck) =>
            Then(exceptionCheck);

        public IThenContinuationBuilder<TContext> And(Action<TContext, Result> exceptionCheck, string name) => 
            Then(exceptionCheck, name);

        public IThenContinuationBuilder<TContext> GetContinuationBuilder() => this;

        public TestableScenario Create([CallerMemberName] string name = null) => CreateTestableScenario(name);

        public void Test([CallerMemberName] string name = null) => CreateTestableScenario(name)
            .RunTest()
            .PublishDoc(_docPublisher, CancellationToken.None)
            .ThrowOnErrors();

        private TestableScenario CreateTestableScenario(string defaultName)
        {
            _isCompleted = true;
            return new TestableScenario(
                new Scenario(_feature, _rule, _name ?? defaultName, _description, _givenSteps, _whenStep, _thenSteps),
                new ScenarioTest<TContext>(_context, _givenActions, _whenAction, _thenActions, _exceptionChecks)
            );
        }
    }
}