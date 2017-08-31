using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ITLibrium.Bdd.Scenarios
{
    public interface IThenContinuationBuilder<TFixture>
    {
        IThenContinuationBuilder<TFixture> And(Expression<Action<TFixture>> thenAction);
        IThenContinuationBuilder<TFixture> And(Action<TFixture> thenAction, string name);
        IThenContinuationBuilder<TFixture> And(Expression<Action<TFixture, Exception>> thenAction);
        IThenContinuationBuilder<TFixture> And(Action<TFixture, Exception> thenAction, string name);

        IBddScenario Create([CallerMemberName] string title = null);
        void Test([CallerMemberName] string title = null);
    }
}