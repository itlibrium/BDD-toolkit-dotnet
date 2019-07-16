using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace ITLibrium.Bdd.Scenarios
{
    public interface IThenBuilder<TContext>
    {
        [PublicAPI]
        IThenContinuationBuilder<TContext> Then(Expression<Action<TContext>> thenAction);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> Then(Action<TContext> thenAction, string name);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> Then(Expression<Action<TContext, Exception>> thenAction);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> Then(Action<TContext, Exception> thenAction, string name);

        IThenContinuationBuilder<TContext> GetContinuationBuilder();
    }
}