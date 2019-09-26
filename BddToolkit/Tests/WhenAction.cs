using System;

namespace ITLIBRIUM.BddToolkit.Tests
{
    internal class WhenAction<TContext>
    {
        private readonly Action<TContext> _action;

        public WhenAction(Action<TContext> action) => _action = action;

        public void Execute(TContext context) => _action(context);
    }
}