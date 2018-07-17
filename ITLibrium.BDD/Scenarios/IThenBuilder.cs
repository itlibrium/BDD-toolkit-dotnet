using System;
using System.Linq.Expressions;

namespace ITLibrium.Bdd.Scenarios
{
    public interface IThenBuilder<TContext>
    {
        IThenContinuationBuilder<TContext> Then(Expression<Action<TContext>> thenAction);
        IThenContinuationBuilder<TContext> Then(Action<TContext> thenAction, string name);
        IThenContinuationBuilder<TContext> Then(Expression<Action<TContext, Exception>> thenAction);
        IThenContinuationBuilder<TContext> Then(Action<TContext, Exception> thenAction, string name);

        IThenContinuationBuilder<TContext> GetContinuationBuilder();
    }
}