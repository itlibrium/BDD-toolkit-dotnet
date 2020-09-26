using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Builders.Scenarios
{
    public interface IGivenContinuationBuilder<TContext> : IWhenBuilder<TContext>
    {
        [PublicAPI]
        IGivenContinuationBuilder<TContext> And(Expression<Action<TContext>> givenAction);
        
        [PublicAPI]
        IGivenContinuationBuilder<TContext> And(Expression<Func<TContext, Task>> givenAction);
        
        [PublicAPI]
        IGivenContinuationBuilder<TContext> And(Action<TContext> givenAction, string name);
        
        [PublicAPI]
        IGivenContinuationBuilder<TContext> And(Func<TContext, Task> givenAction, string name);
    }
}