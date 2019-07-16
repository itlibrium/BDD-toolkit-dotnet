using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace ITLibrium.Bdd.Scenarios
{
    public interface IGivenContinuationBuilder<TContext> : IWhenBuilder<TContext>
    {
        [PublicAPI]
        IGivenContinuationBuilder<TContext> And(Expression<Action<TContext>> givenAction);
        
        [PublicAPI]
        IGivenContinuationBuilder<TContext> And(Action<TContext> givenAction, string name);
    }
}