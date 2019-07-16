using System;

namespace ITLibrium.Bdd.Scenarios
{
    internal class ThenAction<TContext> : BddAction
    {
        private readonly Action<TContext, Exception> _action;

        public ThenAction(Action<TContext, Exception> action, string name) : base(name) => _action = action;

        public void Execute(TContext fixture, Exception whenException) => _action(fixture, whenException);
    }
}