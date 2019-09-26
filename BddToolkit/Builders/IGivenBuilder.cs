using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Builders
{
    public interface IGivenBuilder<TContext> : IWhenBuilder<TContext>
    {
        [PublicAPI]
        IGivenContinuationBuilder<TContext> Given(Expression<Action<TContext>> action);
        
        [PublicAPI]
        IGivenContinuationBuilder<TContext> Given(Action<TContext> action, string name);
    }
}