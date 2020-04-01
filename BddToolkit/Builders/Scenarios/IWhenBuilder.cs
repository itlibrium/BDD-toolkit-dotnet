using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Builders.Scenarios
{
    public interface IWhenBuilder<TContext>
    {
        [PublicAPI]
        IThenBuilder<TContext> When(Expression<Action<TContext>> action);
        
        [PublicAPI]
        IThenBuilder<TContext> When(Action<TContext> action, string name);
    }
}