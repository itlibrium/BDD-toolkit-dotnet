using System;
using System.Linq.Expressions;

namespace ITLibrium.Bdd.Scenarios
{
    public interface IWhenBuilder<TContext>
    {
        IThenBuilder<TContext> When(Expression<Action<TContext>> whenAction);
        IThenBuilder<TContext> When(Action<TContext> whenAction, string name);
    }
}