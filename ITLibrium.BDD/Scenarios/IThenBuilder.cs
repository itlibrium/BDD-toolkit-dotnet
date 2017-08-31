using System;
using System.Linq.Expressions;

namespace ITLibrium.Bdd.Scenarios
{
    public interface IThenBuilder<TFixture>
    {
        IThenContinuationBuilder<TFixture> Then(Expression<Action<TFixture>> thenAction);
        IThenContinuationBuilder<TFixture> Then(Action<TFixture> thenAction, string name);
        IThenContinuationBuilder<TFixture> Then(Expression<Action<TFixture, Exception>> thenAction);
        IThenContinuationBuilder<TFixture> Then(Action<TFixture, Exception> thenAction, string name);

        IThenContinuationBuilder<TFixture> GetContinuationBuilder();
    }
}