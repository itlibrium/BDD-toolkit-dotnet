using System;

namespace ITLIBRIUM.BddToolkit.Tests
{
    internal class GivenAction<TContext>
    {
        private readonly Action<TContext> _action;

        public GivenAction(Action<TContext> action) => _action = action;

        public void Execute(TContext context) => _action(context);
    }
}