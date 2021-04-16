using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Builders.Scenarios
{
    public interface IGivenBuilder<TContext> : IWhenBuilder<TContext>
    {
        [PublicAPI]
        IGivenContinuationBuilder<TContext> Given(Expression<Action<TContext>> action);
        
        [PublicAPI]
        IGivenContinuationBuilder<TContext> Given(Expression<Func<TContext, Task>> action);
        
        [PublicAPI]
        [Obsolete("Use overload with name as a first argument.")]
        IGivenContinuationBuilder<TContext> Given(Action<TContext> action, string name);
        
        [PublicAPI]
        IGivenContinuationBuilder<TContext> Given(string name, Action<TContext> action);
        
        [PublicAPI]
        [Obsolete("Use overload with name as a first argument.")]
        IGivenContinuationBuilder<TContext> Given(Func<TContext, Task> action, string name);
        
        [PublicAPI]
        IGivenContinuationBuilder<TContext> Given(string name, Func<TContext, Task> action);
    }
}