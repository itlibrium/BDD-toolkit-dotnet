using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ITLIBRIUM.BddToolkit.Tests
{
    public interface ScenarioTest
    {
        TestResult Run();
    }

    internal class ScenarioTest<TContext> : ScenarioTest
    {
        private readonly TContext _context;
        private readonly ImmutableArray<GivenAction<TContext>> _given;
        private readonly WhenAction<TContext> _when;
        private readonly ImmutableArray<ThenAction<TContext>> _then;
        private readonly ImmutableArray<ExceptionCheck<TContext>> _exceptionChecks;

        public ScenarioTest(TContext context, IEnumerable<GivenAction<TContext>> given, WhenAction<TContext> @when, 
            IEnumerable<ThenAction<TContext>> then, IEnumerable<ExceptionCheck<TContext>> exceptionChecks)
        {
            _context = context;
            _given = given.ToImmutableArray();
            _when = when;
            _then = then.ToImmutableArray();
            _exceptionChecks = exceptionChecks.ToImmutableArray();
        }

        public TestResult Run()
        {
            var givenActionsResult = ExecuteGivenActions();
            if (!givenActionsResult.IsSuccessful)
                return TestResult.Failed(givenActionsResult.Exception);
                
            var whenActionResult = ExecuteWhenAction();
            
            var exceptions = ImmutableArray.CreateBuilder<Exception>();
            ExecuteThenActions(exceptions);
            CheckExceptions(whenActionResult, exceptions);
            return exceptions.Count == 0
                ? TestResult.Passed()
                : TestResult.Failed(exceptions.ToImmutable());
        }

        private Result ExecuteGivenActions()
        {
            foreach (var action in _given)
            {
                try
                {
                    action(_context);
                }
                catch (Exception e)
                {
                    return Result.Failure(e);
                }
            }
            return Result.Success();
        }

        private Result ExecuteWhenAction()
        {
            try
            {
                _when(_context);
                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failure(e);
            }
        }

        private void ExecuteThenActions(ImmutableArray<Exception>.Builder exceptions)
        {
            foreach (var action in _then)
            {
                try
                {
                    action(_context);
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }
        }

        private void CheckExceptions(Result whenActionResult, ImmutableArray<Exception>.Builder exceptions)
        {
            if (_exceptionChecks.IsEmpty)
            {
                if (!whenActionResult.IsSuccessful) exceptions.Add(whenActionResult.Exception);
            }
            else
            {
                foreach (var check in _exceptionChecks)
                {
                    try
                    {
                        check(_context, whenActionResult);
                    }
                    catch (Exception e)
                    {
                        exceptions.Add(e);
                    }
                }
            }
        }
    }
}