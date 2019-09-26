using System;

namespace ITLIBRIUM.BddToolkit.Tests
{
    internal class ExceptionCheck<TContext>
    {
        private readonly Action<TContext, WhenActionResult> _action;

        public ExceptionCheck(Action<TContext, WhenActionResult> action) => _action = action;

        internal void Execute(TContext context, WhenActionResult result) => _action(context, result);
    }
}