using System;

namespace ITLibrium.Bdd.Scenarios
{
    internal class WhenAction<TFixture> : BddAction
    {
        private readonly Action<TFixture> _action;

        public WhenAction(Action<TFixture> action, string name) 
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