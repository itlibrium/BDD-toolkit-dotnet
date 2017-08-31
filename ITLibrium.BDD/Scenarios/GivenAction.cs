using System;

namespace ITLibrium.Bdd.Scenarios
{
    internal class GivenAction<TFixture> : BddAction
    {
        private readonly Action<TFixture> _action;

        public GivenAction(Action<TFixture> action, string name) 
            : base(name)
        {
            _action = action;
        }

        public void Execute(TFixture fixture)
        {
            _action(fixture);
        }
    }
}