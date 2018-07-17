using System;
using System.Linq.Expressions;

namespace ITLibrium.Bdd.Scenarios
{
    public interface IGivenContinuationBuilder<TContext> : IWhenBuilder<TContext>
    {
        IGivenContinuationBuilder<TContext> And(Expression<Action<TContext>> givenAction);
        IGivenContinuationBuilder<TContext> And(Action<TContext> givenAction, string name);
    }
}