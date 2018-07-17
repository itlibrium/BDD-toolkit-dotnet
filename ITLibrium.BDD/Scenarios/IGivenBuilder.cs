using System;
using System.Linq.Expressions;

namespace ITLibrium.Bdd.Scenarios
{
    public interface IGivenBuilder<TContext> : IWhenBuilder<TContext>
    {
        IGivenContinuationBuilder<TContext> Given(Expression<Action<TContext>> givenAction);
        IGivenContinuationBuilder<TContext> Given(Action<TContext> givenAction, string name);
    }
}