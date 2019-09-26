using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using ITLIBRIUM.BddToolkit.Execution;
using ITLIBRIUM.BddToolkit.Tests;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Builders
{
    public interface IThenContinuationBuilder<TContext>
    {
        [PublicAPI]
        IThenContinuationBuilder<TContext> And(Expression<Action<TContext>> thenAction);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> And(Action<TContext> thenAction, string name);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> And(Expression<Action<TContext, WhenActionResult>> exceptionCheck);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> And(Action<TContext, WhenActionResult> exceptionCheck, string name);

        [PublicAPI]
        TestableScenario Create([CallerMemberName] string name = null);
        
        [PublicAPI]
        void Test([CallerMemberName] string name = null);
    }
}