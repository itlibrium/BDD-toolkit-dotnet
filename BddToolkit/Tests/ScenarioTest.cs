using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using ITLIBRIUM.BddToolkit.Tests.Results;

namespace ITLIBRIUM.BddToolkit.Tests
{
    public interface ScenarioTest
    {
        TestResult Run();
    }

    internal partial class ScenarioTest<TContext> : ScenarioTest
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
            if (givenActionsResult.Failed)
                return TestResult.ExceptionInGivenAction(givenActionsResult.Exception);

            var whenActionResult = ExecuteWhenAction();
            if (whenActionResult.Failed)
            {
                if (_exceptionChecks.IsEmpty)
                    return TestResult.UncheckedExceptionInWhenAction(whenActionResult.Exception);

                var exceptionChecks = Assertions.Check(_exceptionChecks, _context, whenActionResult);
                if (exceptionChecks.Failed)
                    return TestResult.UnexpectedExceptionInWhenAction(whenActionResult.Exception,
                        exceptionChecks.Exceptions);
            }

            var assertions = Assertions.Check(_then, _context);
            return assertions.Failed
                ? TestResult.Failed(assertions.Exceptions)
                : TestResult.Passed();
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
    }
}