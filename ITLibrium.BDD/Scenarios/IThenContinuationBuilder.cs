using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ITLibrium.Bdd.Scenarios
{
    public interface IThenContinuationBuilder<TContext>
    {
        IThenContinuationBuilder<TContext> And(Expression<Action<TContext>> thenAction);
        IThenContinuationBuilder<TContext> And(Action<TContext> thenAction, string name);
        IThenContinuationBuilder<TContext> And(Expression<Action<TContext, Exception>> thenAction);
        IThenContinuationBuilder<TContext> And(Action<TContext, Exception> thenAction, string name);

        IBddScenario Create([CallerMemberName] string title = null);
        void Test([CallerMemberName] string title = null);
    }
}