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
        IGivenContinuationBuilder<TContext> Given(Action<TContext> action, string name);
        
        [PublicAPI]
        IGivenContinuationBuilder<TContext> Given(Func<TContext, Task> action, string name);
    }
}