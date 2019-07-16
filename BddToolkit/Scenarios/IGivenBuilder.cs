using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Scenarios
{
    public interface IGivenBuilder<TContext> : IWhenBuilder<TContext>
    {
        [PublicAPI]
        IGivenContinuationBuilder<TContext> Given(Expression<Action<TContext>> givenAction);
        
        [PublicAPI]
        IGivenContinuationBuilder<TContext> Given(Action<TContext> givenAction, string name);
    }
}