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
        [Obsolete("Use overload with name as a first argument.")]
        IThenBuilder<TContext> When(Action<TContext> action, string name);
        
        [PublicAPI]
        IThenBuilder<TContext> When(string name, Action<TContext> action);
        
        [PublicAPI]
        [Obsolete("Use overload with name as a first argument.")]
        IThenBuilder<TContext> When(Func<TContext, Task> action, string name);
        
        [PublicAPI]
        IThenBuilder<TContext> When(string name, Func<TContext, Task> action);
    }
}