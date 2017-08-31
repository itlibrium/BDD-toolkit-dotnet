using System;

namespace ITLibrium.Bdd.Scenarios
{
    internal class ThenAction<TFixture> : BddAction
    {
        private readonly Action<TFixture, Exception> _action;

        public ThenAction(Action<TFixture, Exception> action, string name) 
            : base(name)
        {
            _action = action;
        }

        public void Execute(TFixture fixture, Exception whenException)
        {
            _action(fixture, whenException);
        }
    }
}