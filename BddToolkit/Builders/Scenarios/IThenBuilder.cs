using System;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Builders.Scenarios
{
    public interface IThenBuilder<TContext>
    {
        [PublicAPI]
        IThenContinuationBuilder<TContext> Then(Expression<Action<TContext>> action);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> Then(Action<TContext> action, string name);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> Then(Expression<Action<TContext, Result>> exceptionCheck);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> Then(Action<TContext, Result> exceptionCheck, string name);

        IThenContinuationBuilder<TContext> GetContinuationBuilder();
    }
}