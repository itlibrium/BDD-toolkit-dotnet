using System;

namespace ITLIBRIUM.BddToolkit.Tests
{
    internal class ThenAction<TContext>
    {
        private readonly Action<TContext> _action;

        public ThenAction(Action<TContext> action) => _action = action;

        internal void Execute(TContext context) => _action(context);
    }
}