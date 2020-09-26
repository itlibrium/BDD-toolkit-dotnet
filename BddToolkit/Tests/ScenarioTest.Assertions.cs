using System;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace ITLIBRIUM.BddToolkit.Tests
{
    internal partial class ScenarioTest<TContext>
    {
        private readonly struct Assertions
        {
            public ImmutableArray<Exception> Exceptions { get; }
            public bool Failed => Exceptions.Length > 0;

            private Assertions(ImmutableArray<Exception> exceptions) => Exceptions = exceptions;

            public static async Task<Assertions> Check(ImmutableArray<ExceptionCheck<TContext>> actions, TContext context,
                Result whenActionResult)
            {
                var exceptions = ImmutableArray.CreateBuilder<Exception>();
                foreach (var action in actions)
                {
                    try
                    {
                        await action(context, whenActionResult);
                    }
                    catch (Exception exception)
                    {
                        exceptions.Add(exception);
                    }
                }
                return new Assertions(exceptions.ToImmutable());
            }

            public static async Task<Assertions> Check(ImmutableArray<ThenAction<TContext>> actions, TContext context)
            {
                var exceptions = ImmutableArray.CreateBuilder<Exception>();
                foreach (var action in actions)
                {
                    try
                    {
                        await action(context);
                    }
                    catch (Exception exception)
                    {
                        exceptions.Add(exception);
                    }
                }
                return new Assertions(exceptions.ToImmutable());
            }
        }
    }
}