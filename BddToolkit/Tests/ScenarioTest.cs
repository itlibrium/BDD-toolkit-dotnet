using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using ITLIBRIUM.BddToolkit.Tests.Results;

namespace ITLIBRIUM.BddToolkit.Tests
{
    public interface ScenarioTest
    {
        TestResult Run();
        Task<TestResult> RunAsync();
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

        public TestResult Run() => RunAsync().GetAwaiter().GetResult();

        public async Task<TestResult> RunAsync()
        {
            var givenActionsResult = await ExecuteGivenActions();
            if (givenActionsResult.Failed)
                return TestResult.ExceptionInGivenAction(givenActionsResult.Exception);

            var whenActionResult = await ExecuteWhenAction();
            if (whenActionResult.Failed && _exceptionChecks.IsEmpty)
                return TestResult.UncheckedExceptionInWhenAction(whenActionResult.Exception);

            var exceptionChecks = await Assertions.Check(_exceptionChecks, _context, whenActionResult);
            if (exceptionChecks.Failed)
            {
                if (whenActionResult.IsSuccessful)
                {
                    return TestResult.NoExpectedExceptionInWhenAction(exceptionChecks.Exceptions);
                }
                return TestResult.UnexpectedExceptionInWhenAction(whenActionResult.Exception,
                    exceptionChecks.Exceptions);
            }

            var assertions = await Assertions.Check(_then, _context);
            return assertions.Failed
                ? TestResult.Failed(assertions.Exceptions)
                : TestResult.Passed();
        }

        private async Task<Result> ExecuteGivenActions()
        {
            foreach (var action in _given)
            {
                try
                {
                    await action(_context);
                }
                catch (Exception e)
                {
                    return Result.Failure(e);
                }
            }

            return Result.Success();
        }

        private async Task<Result> ExecuteWhenAction()
        {
            if (_when is null)
                return Result.Success();
            try
            {
                await _when(_context);
                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failure(e);
            }
        }
    }
}