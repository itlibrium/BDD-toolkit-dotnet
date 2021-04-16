using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ITLIBRIUM.BddToolkit.Execution;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Builders.Scenarios
{
    public interface IThenContinuationBuilder<TContext>
    {
        [PublicAPI]
        IThenContinuationBuilder<TContext> And(Expression<Action<TContext>> thenAction);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> And(Expression<Func<TContext, Task>> thenAction);
        
        [PublicAPI]
        [Obsolete("Use overload with name as a first argument.")]
        IThenContinuationBuilder<TContext> And(Action<TContext> thenAction, string name);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> And(string name, Action<TContext> thenAction);
        
        [PublicAPI]
        [Obsolete("Use overload with name as a first argument.")]
        IThenContinuationBuilder<TContext> And(Func<TContext, Task> thenAction, string name);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> And(string name, Func<TContext, Task> thenAction);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> And(Expression<Action<TContext, Result>> exceptionCheck);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> And(Expression<Func<TContext, Result, Task>> exceptionCheck);
        
        [PublicAPI]
        [Obsolete("Use overload with name as a first argument.")]
        IThenContinuationBuilder<TContext> And(Action<TContext, Result> exceptionCheck, string name);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> And(string name, Action<TContext, Result> exceptionCheck);
        
        [PublicAPI]
        [Obsolete("Use overload with name as a first argument.")]
        IThenContinuationBuilder<TContext> And(Func<TContext, Result, Task> exceptionCheck, string name);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> And(string name, Func<TContext, Result, Task> exceptionCheck);

        [PublicAPI]
        TestableScenario Create([CallerMemberName] string name = null);
        
        [PublicAPI]
        void Test([CallerMemberName] string name = null);
        
        [PublicAPI]
        Task TestAsync([CallerMemberName] string name = null);
    }
}