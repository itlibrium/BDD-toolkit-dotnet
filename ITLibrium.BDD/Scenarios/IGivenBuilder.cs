using System;
using System.Linq.Expressions;

namespace ITLibrium.Bdd.Scenarios
{
    public interface IGivenBuilder<TFixture>
    {
        IGivenContinuationBuilder<TFixture> Given(Expression<Action<TFixture>> givenAction);
        IGivenContinuationBuilder<TFixture> Given(Action<TFixture> givenAction, string name);

        IWhenBuilder<TFixture> GivenNoAction();
    }
}