using System;
using System.Linq.Expressions;
using ITLIBRIUM.BddToolkit.Tests;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Builders
{
    public interface IThenBuilder<TContext>
    {
        [PublicAPI]
        IThenContinuationBuilder<TContext> Then(Expression<Action<TContext>> action);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> Then(Action<TContext> action, string name);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> Then(Expression<Action<TContext, WhenActionResult>> exceptionCheck);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> Then(Action<TContext, WhenActionResult> exceptionCheck, string name);

        IThenContinuationBuilder<TContext> GetContinuationBuilder();
    }
}