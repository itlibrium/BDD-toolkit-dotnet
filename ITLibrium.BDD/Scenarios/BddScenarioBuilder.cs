using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Humanizer;
using ITLibrium.Bdd.Reports;
using ITLibrium.Reflection;

namespace ITLibrium.Bdd.Scenarios
{
    internal class BddScenarioBuilder<TContext> :
        IGivenBuilder<TContext>,
        IGivenContinuationBuilder<TContext>,
        IThenBuilder<TContext>,
        IThenContinuationBuilder<TContext>
    {
        private readonly string _testedComponent;
        private readonly string _title;

        private readonly TContext _context;

        private readonly bool _excludeFromReport;
        private readonly IReadOnlyList<IBddReport> _reports;

        private readonly List<GivenAction<TContext>> _givenActions = new List<GivenAction<TContext>>();
        private WhenAction<TContext> _whenAction;
        private readonly List<ThenAction<TContext>> _thenActions = new List<ThenAction<TContext>>();

        private bool _exceptionsAreExplicitlyChecked;

        private bool _testCreated;

        public BddScenarioBuilder(string testedComponent, string title, TContext context, bool excludeFromReports,
            IReadOnlyList<IBddReport> reports)
        {
            _testedComponent = testedComponent;
            _title = title;

            _context = context;

            _excludeFromReport = excludeFromReports;
            _reports = reports;
        }

        ~BddScenarioBuilder()
        {
            if (!_testCreated)
                throw new InvalidOperationException(
                    "BddScenarioBuilder should create scenario. Call Create or Test method afther Then section");
        }

        public IGivenContinuationBuilder<TContext> Given(Expression<Action<TContext>> givenAction)
        {
            return Given(givenAction.Compile(), givenAction.GetName());
        }

        public IGivenContinuationBuilder<TContext> Given(Action<TContext> givenAction, string name)
        {
            _givenActions.Add(new GivenAction<TContext>(givenAction, name));
            return this;
        }

        IGivenContinuationBuilder<TContext> IGivenContinuationBuilder<TContext>.And(
            Expression<Action<TContext>> givenAction)
        {
            return Given(givenAction);
        }

        IGivenContinuationBuilder<TContext> IGivenContinuationBuilder<TContext>.And(Action<TContext> givenAction,
            string name)
        {
            return Given(givenAction, name);
        }

        public IThenBuilder<TContext> When(Expression<Action<TContext>> whenAction)
        {
            return When(whenAction.Compile(), whenAction.GetName());
        }

        public IThenBuilder<TContext> When(Action<TContext> whenAction, string name)
        {
            _whenAction = new WhenAction<TContext>(whenAction, name);
            return this;
        }

        public IThenContinuationBuilder<TContext> Then(Expression<Action<TContext>> thenAction)
        {
            return Then(thenAction.Compile(), thenAction.GetName());
        }

        public IThenContinuationBuilder<TContext> Then(Action<TContext> thenAction, string name)
        {
            return Then((f, e) => thenAction(f), name, false);
        }

        public IThenContinuationBuilder<TContext> Then(Expression<Action<TContext, Exception>> thenAction)
        {
            return Then(thenAction.Compile(), thenAction.GetName());
        }

        public IThenContinuationBuilder<TContext> Then(Action<TContext, Exception> thenAction, string name)
        {
            return Then(thenAction, name, true);
        }

        private IThenContinuationBuilder<TContext> Then(Action<TContext, Exception> thenAction, string name,
            bool exceptionsAreExplicitlyChecked)
        {
            _thenActions.Add(new ThenAction<TContext>(thenAction, name));
            _exceptionsAreExplicitlyChecked = exceptionsAreExplicitlyChecked;
            return this;
        }

        IThenContinuationBuilder<TContext> IThenContinuationBuilder<TContext>.And(
            Expression<Action<TContext>> thenAction)
        {
            return Then(thenAction);
        }

        IThenContinuationBuilder<TContext> IThenContinuationBuilder<TContext>.And(Action<TContext> thenAction,
            string name)
        {
            return Then(thenAction, name);
        }

        IThenContinuationBuilder<TContext> IThenContinuationBuilder<TContext>.And(
            Expression<Action<TContext, Exception>> thenAction)
        {
            return Then(thenAction);
        }

        public IThenContinuationBuilder<TContext> And(Action<TContext, Exception> thenAction, string name)
        {
            return Then(thenAction, name);
        }

        public IThenContinuationBuilder<TContext> GetContinuationBuilder()
        {
            return this;
        }

        public IBddScenario Create([CallerMemberName] string title = null)
        {
            return CreateInternal(title.Humanize());
        }

        public void Test([CallerMemberName] string title = null)
        {
            CreateInternal(title.Humanize()).Test();
        }

        private IBddScenario CreateInternal(string title)
        {
            _testCreated = true;
            return new BddScenarioImpl<TContext>(_testedComponent, _title ?? title, _context,
                _excludeFromReport, _reports, _givenActions, _whenAction, _thenActions,
                _exceptionsAreExplicitlyChecked);
        }
    }
}