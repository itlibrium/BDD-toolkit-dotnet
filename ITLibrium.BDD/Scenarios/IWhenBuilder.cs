using System;
using System.Linq.Expressions;

namespace ITLibrium.Bdd.Scenarios
{
    public interface IWhenBuilder<TFixture>
    {
        IThenBuilder<TFixture> When(Expression<Action<TFixture>> whenAction);
        IThenBuilder<TFixture> When(Action<TFixture> whenAction, string name);
    }
}