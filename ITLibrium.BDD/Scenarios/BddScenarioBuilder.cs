using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Humanizer;
using ITLibrium.Reflection;

namespace ITLibrium.Bdd.Scenarios
{
    internal class BddScenarioBuilder<TFixture> :
        IGivenBuilder<TFixture>, 
        IGivenContinuationBuilder<TFixture>,
        IThenBuilder<TFixture>, 
        IThenContinuationBuilder<TFixture>
    {
        private readonly TFixture _fixture;
        private readonly string _title;

        private readonly List<GivenAction<TFixture>> _givenActions = new List<GivenAction<TFixture>>();
        private WhenAction<TFixture> _whenAction;
        private readonly List<ThenAction<TFixture>> _thenActions = new List<ThenAction<TFixture>>();

        public BddScenarioBuilder(TFixture fixture, string title = null)
        {
            _fixture = fixture;
            _title = title;
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

        public IWhenBuilder<TFixture> GivenNoAction()
        {
            return this;
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
            return Then((f, e) => thenAction(f), name);
        }

        public IThenContinuationBuilder<TFixture> Then(Expression<Action<TFixture, Exception>> thenAction)
        {
            return Then(thenAction.Compile(), thenAction.GetName());
        }

        public IThenContinuationBuilder<TFixture> Then(Action<TFixture, Exception> thenAction, string name)
        {
            _thenActions.Add(new ThenAction<TFixture>(thenAction, name));
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
            return new BddScenarioImpl<TFixture>(_title ?? title, _fixture, _givenActions, _whenAction, _thenActions);
        }
    }
}