﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Humanizer;
using ITLIBRIUM.BddToolkit.Docs;
using ITLIBRIUM.BddToolkit.Execution;
using ITLIBRIUM.BddToolkit.Syntax.Features;
using ITLIBRIUM.BddToolkit.Syntax.Rules;
using ITLIBRIUM.BddToolkit.Syntax.Scenarios;
using ITLIBRIUM.BddToolkit.Tests;
using ITLIBRIUM.ReflectionToolkit;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Builders.Scenarios
{
    internal class ScenarioBuilder<TContext> :
        IFeatureAndRuleBuilder<TContext>,
        IGivenContinuationBuilder<TContext>,
        IThenContinuationBuilder<TContext>
        where TContext : class
    {
        private readonly TContext _context;
        private readonly DocPublisher _docPublisher;

        private Feature _feature = Syntax.Features.Feature.Empty();
        private Rule _rule = Syntax.Rules.Rule.Empty();
        private string _name;
        private string _description;
        private string[] _tags;
        private readonly List<GivenStep> _givenSteps = new List<GivenStep>();
        private readonly List<GivenAction<TContext>> _givenActions = new List<GivenAction<TContext>>();
        private WhenStep? _whenStep;
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

        public INameBuilder<TContext> Feature(in Feature feature)
        {
            _feature = feature;
            return this;
        }

        public INameBuilder<TContext> Rule(in Rule rule)
        {
            _feature = rule.Feature;
            _rule = rule;
            return this;
        }

        public IDescriptionBuilder<TContext> Name(string name)
        {
            _name = name;
            return this;
        }

        public ITagsBuilder<TContext> Description(string description)
        {
            _description = description;
            return this;
        }

        public IGivenBuilder<TContext> Tags(params string[] tags)
        {
            _tags = tags;
            return this;
        }

        public IGivenContinuationBuilder<TContext> Given(Expression<Action<TContext>> action) =>
            Given(GetName(action), action.Compile());

        public IGivenContinuationBuilder<TContext> Given(Expression<Func<TContext, Task>> action) =>
            Given(GetName(action), action.Compile());

        [Obsolete]
        public IGivenContinuationBuilder<TContext> Given(Action<TContext> action, string name) =>
            Given(name, ToAsyncAction(action));
        
        public IGivenContinuationBuilder<TContext> Given(string name, Action<TContext> action) =>
            Given(name, ToAsyncAction(action));

        [Obsolete]
        public IGivenContinuationBuilder<TContext> Given(Func<TContext, Task> action, string name) =>
            Given(name, action);
        
        public IGivenContinuationBuilder<TContext> Given(string name, Func<TContext, Task> action)
        {
            _givenSteps.Add(new GivenStep(name));
            _givenActions.Add(new GivenAction<TContext>(action));
            return this;
        }

        IGivenContinuationBuilder<TContext> IGivenContinuationBuilder<TContext>.And(
            Expression<Action<TContext>> action) =>
            Given(action);

        IGivenContinuationBuilder<TContext> IGivenContinuationBuilder<TContext>.And(
            Expression<Func<TContext, Task>> action) =>
            Given(action);

        [Obsolete]
        IGivenContinuationBuilder<TContext> IGivenContinuationBuilder<TContext>.And(
            Action<TContext> action, string name) =>
            Given(name, action);
        
        IGivenContinuationBuilder<TContext> IGivenContinuationBuilder<TContext>.And(
            string name, Action<TContext> action) =>
            Given(name, action);

        [Obsolete]
        IGivenContinuationBuilder<TContext> IGivenContinuationBuilder<TContext>.And(
            Func<TContext, Task> action, string name) =>
            Given(name, action);
        
        IGivenContinuationBuilder<TContext> IGivenContinuationBuilder<TContext>.And(
            string name, Func<TContext, Task> action) =>
            Given(name, action);

        public IThenBuilder<TContext> When(Expression<Action<TContext>> action) =>
            When(GetName(action), action.Compile());

        public IThenBuilder<TContext> When(Expression<Func<TContext, Task>> action) =>
            When(GetName(action), action.Compile());

        [Obsolete]
        public IThenBuilder<TContext> When(Action<TContext> action, string name) =>
            When(name, ToAsyncAction(action));
        
        public IThenBuilder<TContext> When(string name, Action<TContext> action) =>
            When(name, ToAsyncAction(action));

        [Obsolete]
        public IThenBuilder<TContext> When(Func<TContext, Task> action, string name) =>
            When(name, action);
        
        public IThenBuilder<TContext> When(string name, Func<TContext, Task> action)
        {
            _whenStep = new WhenStep(name);
            _whenAction = new WhenAction<TContext>(action);
            return this;
        }

        public IThenContinuationBuilder<TContext> Then(Expression<Action<TContext>> action) =>
            Then(GetName(action), action.Compile());

        public IThenContinuationBuilder<TContext> Then(Expression<Func<TContext, Task>> action) =>
            Then(GetName(action), action.Compile());

        [Obsolete]
        public IThenContinuationBuilder<TContext> Then(Action<TContext> thenAction, string name) =>
            Then(name, ToAsyncAction(thenAction));
        
        public IThenContinuationBuilder<TContext> Then(string name, Action<TContext> thenAction) =>
            Then(name, ToAsyncAction(thenAction));

        [Obsolete]
        public IThenContinuationBuilder<TContext> Then(Func<TContext, Task> thenAction, string name) =>
            Then(name, thenAction);
        
        public IThenContinuationBuilder<TContext> Then(string name, Func<TContext, Task> thenAction)
        {
            _thenSteps.Add(new ThenStep(name));
            _thenActions.Add(new ThenAction<TContext>(thenAction));
            return this;
        }

        public IThenContinuationBuilder<TContext> Then(Expression<Action<TContext, Result>> exceptionCheck) =>
            Then(GetName(exceptionCheck), exceptionCheck.Compile());

        public IThenContinuationBuilder<TContext> Then(Expression<Func<TContext, Result, Task>> exceptionCheck) =>
            Then(GetName(exceptionCheck), exceptionCheck.Compile());

        [Obsolete]
        public IThenContinuationBuilder<TContext> Then(Action<TContext, Result> exceptionCheck, string name) =>
            Then(name, ToAsyncAction(exceptionCheck));
        
        public IThenContinuationBuilder<TContext> Then(string name, Action<TContext, Result> exceptionCheck) =>
            Then(name, ToAsyncAction(exceptionCheck));

        [Obsolete]
        public IThenContinuationBuilder<TContext> Then(Func<TContext, Result, Task> exceptionCheck, string name) =>
            Then(name, exceptionCheck);
        
        public IThenContinuationBuilder<TContext> Then(string name, Func<TContext, Result, Task> exceptionCheck)
        {
            _thenSteps.Add(new ThenStep(name));
            _exceptionChecks.Add(new ExceptionCheck<TContext>(exceptionCheck));
            return this;
        }

        IThenContinuationBuilder<TContext> IThenContinuationBuilder<TContext>.And(
            Expression<Action<TContext>> thenAction) =>
            Then(thenAction);

        IThenContinuationBuilder<TContext> IThenContinuationBuilder<TContext>.And(
            Expression<Func<TContext, Task>> thenAction) =>
            Then(thenAction);

        [Obsolete]
        IThenContinuationBuilder<TContext> IThenContinuationBuilder<TContext>.And(
            Action<TContext> thenAction, string name) =>
            Then(name, thenAction);
        
        IThenContinuationBuilder<TContext> IThenContinuationBuilder<TContext>.And(
            string name, Action<TContext> thenAction) =>
            Then(name, thenAction);

        [Obsolete]
        IThenContinuationBuilder<TContext> IThenContinuationBuilder<TContext>.And(
            Func<TContext, Task> thenAction, string name) =>
            Then(name, thenAction);
        
        IThenContinuationBuilder<TContext> IThenContinuationBuilder<TContext>.And(
            string name, Func<TContext, Task> thenAction) =>
            Then(name, thenAction);

        IThenContinuationBuilder<TContext> IThenContinuationBuilder<TContext>.And(
            Expression<Action<TContext, Result>> exceptionCheck) =>
            Then(exceptionCheck);

        IThenContinuationBuilder<TContext> IThenContinuationBuilder<TContext>.And(
            Expression<Func<TContext, Result, Task>> exceptionCheck) =>
            Then(exceptionCheck);

        [Obsolete]
        public IThenContinuationBuilder<TContext> And(Action<TContext, Result> exceptionCheck, string name) =>
            Then(name, exceptionCheck);
        
        public IThenContinuationBuilder<TContext> And(string name, Action<TContext, Result> exceptionCheck) =>
            Then(name, exceptionCheck);

        [Obsolete]
        public IThenContinuationBuilder<TContext> And(Func<TContext, Result, Task> exceptionCheck, string name) =>
            Then(name, exceptionCheck);
        
        public IThenContinuationBuilder<TContext> And(string name, Func<TContext, Result, Task> exceptionCheck) =>
            Then(name, exceptionCheck);

        public IThenContinuationBuilder<TContext> GetContinuationBuilder() => this;

        public TestableScenario Create([CallerMemberName] string name = null) => CreateTestableScenario(name);

        public void Test([CallerMemberName] string name = null) =>
            CreateTestableScenario(name)
                .RunTest()
                .PublishDoc(_docPublisher, CancellationToken.None)
                .ThrowOnErrors();

        public async Task TestAsync([CallerMemberName] string name = null) =>
            (await CreateTestableScenario(name)
                .RunTestAsync())
            .PublishDoc(_docPublisher, CancellationToken.None)
            .ThrowOnErrors();

        private static Func<T, Task> ToAsyncAction<T>(Action<T> action) =>
            t =>
            {
                action(t);
                return Task.CompletedTask;
            };

        private static Func<T1, T2, Task> ToAsyncAction<T1, T2>(Action<T1, T2> action) =>
            (t1, t2) =>
            {
                action(t1, t2);
                return Task.CompletedTask;
            };

        private static string GetName(LambdaExpression lambdaExp)
        {
            var name = lambdaExp.GetName().Humanize(LetterCasing.LowerCase);
            if (lambdaExp.TryGetParameterValues(out var parameterValues))
                name = parameterValues.Aggregate(name, (n, p) => $"{n} {p}");
            return name;
        }

        private TestableScenario CreateTestableScenario(string reflectedName)
        {
            _isCompleted = true;
            return new TestableScenario(
                new Scenario(_feature, _rule,
                    GetScenarioName(reflectedName), _description,
                    _tags ?? Enumerable.Empty<string>(),
                    _givenSteps, _whenStep, _thenSteps),
                new ScenarioTest<TContext>(_context, _givenActions, _whenAction, _thenActions, _exceptionChecks)
            );
        }

        private string GetScenarioName(string reflectedName)
        {
            if (_name != null)
                return _name;

            if (reflectedName != null)
                return reflectedName.Humanize();

            throw new InvalidOperationException("Scenario name is missing.");
        }
    }
}