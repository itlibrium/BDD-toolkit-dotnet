using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace ITLibrium.Bdd.Scenarios
{
    public interface IThenContinuationBuilder<TContext>
    {
        [PublicAPI]
        IThenContinuationBuilder<TContext> And(Expression<Action<TContext>> thenAction);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> And(Action<TContext> thenAction, string name);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> And(Expression<Action<TContext, Exception>> thenAction);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> And(Action<TContext, Exception> thenAction, string name);

        [PublicAPI]
        IBddScenario Create([CallerMemberName] string title = null);
        
        [PublicAPI]
        void Test([CallerMemberName] string title = null);
    }
}