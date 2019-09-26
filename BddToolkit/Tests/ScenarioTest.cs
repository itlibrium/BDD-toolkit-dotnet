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
        private readonly ImmutableArray<ThenAction<TContext>> _thenActions;
        private readonly ImmutableArray<ExceptionCheck<TContext>> _exceptionChecks;

        public ScenarioTest(TContext context, IEnumerable<GivenAction<TContext>> given, WhenAction<TContext> @when, 
            IEnumerable<ThenAction<TContext>> then, IEnumerable<ExceptionCheck<TContext>> thenException)
        {
            _context = context;
            _given = given.ToImmutableArray();
            _when = when;
            _thenActions = then.ToImmutableArray();
            _exceptionChecks = thenException.ToImmutableArray();
        }
        
        //DocBuilder
        //    EnsureFeature / Rule
        //    AddScenario


        public TestResult Run()
        {
            ExecuteGivenSection();
            var whenException = ExecuteWhenSection();
            return ExecuteThenSection(whenException);
        }

        private void ExecuteGivenSection()
        {
            foreach (var action in _given)
                action.Execute(_context);
        }

        private WhenActionResult ExecuteWhenSection()
        {
            try
            {
                _when.Execute(_context);
                return WhenActionResult.Success();
            }
            catch (Exception e)
            {
                return WhenActionResult.Failure(e);
            }
        }

        private TestResult ExecuteThenSection(WhenActionResult whenActionResult)
        {
            var exceptions = new List<Exception>();
            foreach (var action in _thenActions)
            {
                try
                {
                    action.Execute(_context);
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }
            if (_exceptionChecks.IsEmpty)
            {
                if(!whenActionResult.IsSuccessful) exceptions.Add(whenActionResult.Exception);
            }
            else
            {
                foreach (var action in _exceptionChecks)
                {
                    try
                    {
                        action.Execute(_context, whenActionResult);
                    }
                    catch (Exception e)
                    {
                        exceptions.Add(e);
                    }
                }
            }
            return exceptions.Count == 0 ? TestResult.Passed() : TestResult.Failed(exceptions);
        }
    }
}