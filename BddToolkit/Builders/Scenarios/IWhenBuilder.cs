using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Builders.Scenarios
{
    public interface IWhenBuilder<TContext> : IThenBuilder<TContext>
    {
        [PublicAPI]
        IThenBuilder<TContext> When(Expression<Action<TContext>> action);
        
        [PublicAPI]
        IThenBuilder<TContext> When(Expression<Func<TContext, Task>> action);
        
        [PublicAPI]
        IThenBuilder<TContext> When(Action<TContext> action, string name);
        
        [PublicAPI]
        IThenBuilder<TContext> When(Func<TContext, Task> action, string name);
    }
}