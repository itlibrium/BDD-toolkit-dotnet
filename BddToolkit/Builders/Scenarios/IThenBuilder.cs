using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Builders.Scenarios
{
    public interface IThenBuilder<TContext>
    {
        [PublicAPI]
        IThenContinuationBuilder<TContext> Then(Expression<Action<TContext>> action);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> Then(Expression<Func<TContext, Task>> action);
        
        [PublicAPI]
        [Obsolete("Use overload with name as a first argument.")]
        IThenContinuationBuilder<TContext> Then(Action<TContext> action, string name);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> Then(string name, Action<TContext> action);
        
        [PublicAPI]
        [Obsolete("Use overload with name as a first argument.")]
        IThenContinuationBuilder<TContext> Then(Func<TContext, Task> action, string name);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> Then(string name, Func<TContext, Task> action);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> Then(Expression<Action<TContext, Result>> exceptionCheck);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> Then(Expression<Func<TContext, Result, Task>> exceptionCheck);
        
        [PublicAPI]
        [Obsolete("Use overload with name as a first argument.")]
        IThenContinuationBuilder<TContext> Then(Action<TContext, Result> exceptionCheck, string name);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> Then(string name, Action<TContext, Result> exceptionCheck);
        
        [PublicAPI]
        [Obsolete("Use overload with name as a first argument.")]
        IThenContinuationBuilder<TContext> Then(Func<TContext, Result, Task> exceptionCheck, string name);
        
        [PublicAPI]
        IThenContinuationBuilder<TContext> Then(string name, Func<TContext, Result, Task> exceptionCheck);

        IThenContinuationBuilder<TContext> GetContinuationBuilder();
    }
}