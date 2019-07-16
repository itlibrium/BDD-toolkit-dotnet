using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Scenarios
{
    public interface IWhenBuilder<TContext>
    {
        [PublicAPI]
        IThenBuilder<TContext> When(Expression<Action<TContext>> whenAction);
        
        [PublicAPI]
        IThenBuilder<TContext> When(Action<TContext> whenAction, string name);
    }
}