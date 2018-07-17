using System;

namespace ITLibrium.Bdd.Scenarios
{
    internal class WhenAction<TContext> : BddAction
    {
        private readonly Action<TContext> _action;

        public WhenAction(Action<TContext> action, string name) 
            : base(name)
        {
            _action = action;
        }

        public void Execute(TContext fixture)
        {
            _action(fixture);
        }
    }
}