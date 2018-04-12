using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Humanizer;
using ITLibrium.Bdd.Reports;
using ITLibrium.Reflection;

namespace ITLibrium.Bdd.Scenarios
{
    internal class BddScenarioBuilder<TFixture> :
        IGivenBuilder<TFixture>,
        IGivenContinuationBuilder<TFixture>,
        IThenBuilder<TFixture>,
        IThenContinuationBuilder<TFixture>
    {
        private readonly string _testedComponent;
        private readonly string _title;
        
        private readonly TFixture _fixture;
        
        private readonly bool _excludeFromReport;
        private readonly IReadOnlyList<IBddReport> _reports;

        private readonly List<GivenAction<TFixture>> _givenActions = new List<GivenAction<TFixture>>();
        private WhenAction<TFixture> _whenAction;
        private readonly List<ThenAction<TFixture>> _thenActions = new List<ThenAction<TFixture>>();

        private bool _exceptionsAreExplicitlyChecked;

        private bool _testCreated;
        
        public BddScenarioBuilder(string testedComponent, string title, TFixture fixture, bool excludeFromReports, IReadOnlyList<IBddReport> reports)
        {
            _testedComponent = testedComponent;
            _title = title;
            
            _fixture = fixture;
            
            _excludeFromReport = excludeFromReports;
            _reports = reports;
        }

        ~BddScenarioBuilder()
        {
            if(!_testCreated)
                throw new InvalidOperationException("BddScenarioBuilder should create scenario. Call Create or Test method afther Then section");
        }

        public IGivenContinuationBuilder<TFixture> Given(Expression<Action<TFixture>> givenAction)
        {
            return Given(givenAction.Compile(), givenAction.GetName());
        }

        public IGivenContinuationBuilder<TFixture> Given(Action<TFixture> givenAction, string name)
        {
            _givenActions.Add(new GivenAction<TFixture>(givenAction, name));
            return this;
        }

        IGivenContinuationBuilder<TFixture> IGivenContinuationBuilder<TFixture>.And(Expression<Action<TFixture>> givenAction)
        {
            return Given(givenAction);
        }

        IGivenContinuationBuilder<TFixture> IGivenContinuationBuilder<TFixture>.And(Action<TFixture> givenAction, string name)
        {
            return Given(givenAction, name);
        }

        public IThenBuilder<TFixture> When(Expression<Action<TFixture>> whenAction)
        {
            return When(whenAction.Compile(), whenAction.GetName());
        }

        public IThenBuilder<TFixture> When(Action<TFixture> whenAction, string name)
        {
            _whenAction = new WhenAction<TFixture>(whenAction, name);
            return this;
        }

        public IThenContinuationBuilder<TFixture> Then(Expression<Action<TFixture>> thenAction)
        {
            return Then(thenAction.Compile(), thenAction.GetName());
        }

        public IThenContinuationBuilder<TFixture> Then(Action<TFixture> thenAction, string name)
        {
            return Then((f, e) => thenAction(f), name, false);
        }

        public IThenContinuationBuilder<TFixture> Then(Expression<Action<TFixture, Exception>> thenAction)
        {
            return Then(thenAction.Compile(), thenAction.GetName());
        }

        public IThenContinuationBuilder<TFixture> Then(Action<TFixture, Exception> thenAction, string name)
        {
            return Then(thenAction, name, true);
        }

        private IThenContinuationBuilder<TFixture> Then(Action<TFixture, Exception> thenAction, string name, bool exceptionsAreExplicitlyChecked)
        {
            _thenActions.Add(new ThenAction<TFixture>(thenAction, name));
            _exceptionsAreExplicitlyChecked = exceptionsAreExplicitlyChecked;
            return this;
        }

        IThenContinuationBuilder<TFixture> IThenContinuationBuilder<TFixture>.And(Expression<Action<TFixture>> thenAction)
        {
            return Then(thenAction);
        }

        IThenContinuationBuilder<TFixture> IThenContinuationBuilder<TFixture>.And(Action<TFixture> thenAction, string name)
        {
            return Then(thenAction, name);
        }

        IThenContinuationBuilder<TFixture> IThenContinuationBuilder<TFixture>.And(Expression<Action<TFixture, Exception>> thenAction)
        {
            return Then(thenAction);
        }

        public IThenContinuationBuilder<TFixture> And(Action<TFixture, Exception> thenAction, string name)
        {
            return Then(thenAction, name);
        }

        public IThenContinuationBuilder<TFixture> GetContinuationBuilder()
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
            return new BddScenarioImpl<TFixture>(_testedComponent, _title ?? title, _fixture, 
                _excludeFromReport, _reports, _givenActions, _whenAction, _thenActions,
                _exceptionsAreExplicitlyChecked);
        }
    }
}