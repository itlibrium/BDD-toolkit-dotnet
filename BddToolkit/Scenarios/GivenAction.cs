using System;

namespace ITLIBRIUM.BddToolkit.Scenarios
{
    internal class GivenAction<TContext> : BddAction
    {
        private readonly Action<TContext> _action;

        public GivenAction(Action<TContext> action, string name) : base(name) =>
            _action = action;

        public void Execute(TContext fixture) => _action(fixture);
    }
}