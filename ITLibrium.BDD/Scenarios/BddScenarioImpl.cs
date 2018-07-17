﻿using System;
using System.Collections.Generic;
using System.Text;
using Humanizer;
using ITLibrium.Bdd.Reports;

namespace ITLibrium.Bdd.Scenarios
{
    internal class BddScenarioImpl<TContext> : IBddScenario
    {
        private readonly string _testedComponent;
        private readonly string _title;
        
        private readonly TContext _fixture;
        
        private readonly bool _excludeFromReport;
        private readonly IReadOnlyList<IBddReport> _reports;

        private readonly IReadOnlyList<GivenAction<TContext>> _givenActions;
        private readonly WhenAction<TContext> _whenAction;
        private readonly IReadOnlyList<ThenAction<TContext>> _thenActions;

        private readonly bool _exceptionsAreExplicitlyChecked;

        public BddScenarioImpl(string testedComponent, string title, TContext fixture,
            bool excludeFromReport, IReadOnlyList<IBddReport> reports,
            IReadOnlyList<GivenAction<TContext>> givenActions, WhenAction<TContext> whenAction, IReadOnlyList<ThenAction<TContext>> thenActions,
            bool exceptionsAreExplicitlyChecked)
        {
            _testedComponent = testedComponent;
            _title = title;
            _fixture = fixture;
            _excludeFromReport = excludeFromReport;
            _reports = reports;
            _givenActions = givenActions;
            _whenAction = whenAction;
            _thenActions = thenActions;
            _exceptionsAreExplicitlyChecked = exceptionsAreExplicitlyChecked;
        }

        public IBddScenarioDescription GetDescription()
        {
            string givenText = CreateSectionText("Given", _givenActions, "no action");
            string whenText = $"When {_whenAction.Name}".Humanize();
            string thenText = CreateSectionText("Then", _thenActions, "nothing heppened");

            var builder = new StringBuilder();
            builder.Append("Scenario: ");
            builder.AppendLine(_title);
            builder.AppendLine();
            builder.AppendLine(givenText);
            builder.AppendLine(whenText);
            builder.AppendLine(thenText);

            return new BddScenarioDescription(_testedComponent, _title, givenText, whenText, thenText, builder.ToString());
        }

        private static string CreateSectionText(string sectionName, IReadOnlyList<BddAction> actions, string noActionText)
        {
            var builder = new StringBuilder();

            int actionsCount = actions.Count;
            if (actionsCount == 0)
            {
                builder.Append($"{sectionName} {noActionText}");
            }
            else
            {
                builder.Append($"{sectionName} {actions[0].Name}".Humanize());

                for (int i = 1; i < actionsCount; i++)
                {
                    builder.AppendLine();
                    builder.Append('\t');
                    builder.Append($"And {actions[i].Name}".Humanize());
                }               
            }
            
            return builder.ToString();
        }

        public void Test()
        {
            ExecuteGivenSection();
            Exception whenException = ExecuteWhenSection();
            IReadOnlyList<Exception> thenExceptions = ExecuteThenSection(whenException);
            TestResult testResult = CreateResult(whenException, thenExceptions);
            
            AddToReports(testResult);
            
            if (!testResult.Passed)
                throw new AggregateAssertException(testResult.Exceptions);
        }

        private void ExecuteGivenSection()
        {
            foreach (GivenAction<TContext> givenAction in _givenActions)
                givenAction.Execute(_fixture);
        }

        private Exception ExecuteWhenSection()
        {
            try
            {
                var sutCreator = _fixture as ISutCreator;
                sutCreator?.CreateSut();

                _whenAction.Execute(_fixture);
                return null;
            }
            catch (Exception e)
            {
                return e;
            }
        }

        private IReadOnlyList<Exception> ExecuteThenSection(Exception whenException)
        {
            List<Exception> thenExceptions = null;
            foreach (ThenAction<TContext> thenAction in _thenActions)
            {
                try
                {
                    thenAction.Execute(_fixture, whenException);
                }
                catch (Exception e)
                {
                    if (thenExceptions == null)
                        thenExceptions = new List<Exception>();

                    thenExceptions.Add(e);
                }
            }
            return thenExceptions;
        }

        private TestResult CreateResult(Exception whenException, IReadOnlyList<Exception> thenExceptions)
        {
            var result = new TestResult();
            if (whenException != null && !_exceptionsAreExplicitlyChecked)
                result.AddException(whenException);
            
            result.AddExceptions(thenExceptions);
            return result;
        }

        private void AddToReports(TestResult testResult)
        {
            if (_excludeFromReport) 
                return;
            
            IBddScenarioDescription description = GetDescription();
            var scenarioResult = new BddScenarioResult(description, testResult.Passed);
            if (_reports != null && _reports.Count > 0)
            {
                foreach (IBddReport report in _reports)
                    report.AddScenarioResult(scenarioResult);
            }
            else
            {
                BddReport.AddScenarioResult(scenarioResult);
            }
        } 

        public override string ToString() => GetDescription().ToString();
        
        private struct TestResult
        {
            private List<Exception> _exceptions;
            public IEnumerable<Exception> Exceptions => _exceptions;

            public bool Passed => _exceptions == null;

            public void AddException(Exception exception)
            {
                if (exception == null)
                    return;
                
                if (_exceptions == null)
                    _exceptions = new List<Exception>();
                
                _exceptions.Add(exception);
            }
            
            public void AddExceptions(IEnumerable<Exception> exceptions)
            {
                if (exceptions == null)
                    return;
                
                if (_exceptions == null)
                    _exceptions = new List<Exception>();
                
                _exceptions.AddRange(exceptions);
            }
        }
    }
}