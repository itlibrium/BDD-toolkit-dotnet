using System;
using System.Linq.Expressions;

namespace ITLibrium.Bdd.Scenarios
{
    public interface IGivenContinuationBuilder<TFixture> : IWhenBuilder<TFixture>
    {
        IGivenContinuationBuilder<TFixture> And(Expression<Action<TFixture>> givenAction);
        IGivenContinuationBuilder<TFixture> And(Action<TFixture> givenAction, string name);
    }
}