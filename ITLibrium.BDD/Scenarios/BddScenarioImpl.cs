using System;
using System.Collections.Generic;
using System.Text;
using Humanizer;
using ITLibrium.Bdd.Reports;

namespace ITLibrium.Bdd.Scenarios
{
    internal class BddScenarioImpl<TFixture> : IBddScenario
    {
        private readonly string _testedComponent;
        private readonly string _title;
        
        private readonly TFixture _fixture;
        
        private readonly bool _excludeFromReport;
        private readonly IReadOnlyList<IBddReport> _reports;

        private readonly IReadOnlyList<GivenAction<TFixture>> _givenActions;
        private readonly WhenAction<TFixture> _whenAction;
        private readonly IReadOnlyList<ThenAction<TFixture>> _thenActions;

        public BddScenarioImpl(string testedComponent, string title, TFixture fixture,
            bool excludeFromReport, IReadOnlyList<IBddReport> reports,
            IReadOnlyList<GivenAction<TFixture>> givenActions, WhenAction<TFixture> whenAction, IReadOnlyList<ThenAction<TFixture>> thenActions)
        {
            _testedComponent = testedComponent;
            _title = title;
            _fixture = fixture;
            _excludeFromReport = excludeFromReport;
            _reports = reports;
            _givenActions = givenActions;
            _whenAction = whenAction;
            _thenActions = thenActions;
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
            foreach (GivenAction<TFixture> givenAction in _givenActions)
                givenAction.Execute(_fixture);

            Exception whenException = null;
            try
            {
                var sutCreator = _fixture as ISutCreator;
                sutCreator?.CreateSut();

                _whenAction.Execute(_fixture);
            }
            catch (Exception e)
            {
                whenException = e;
            }

            List<Exception> thenExceptions = null;
            foreach (ThenAction<TFixture> thenAction in _thenActions)
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
            bool testPassed = thenExceptions == null;

            if (!_excludeFromReport)
            {
                IBddScenarioDescription description = GetDescription();
                var result = new BddScenarioResult(description, testPassed);
                if (_reports != null && _reports.Count > 0)
                {
                    foreach (IBddReport report in _reports)
                        report.AddScenarioResult(result);
                }
                else
                {
                    BddReport.AddScenarioResult(result);
                }
            }
            
            if (!testPassed)
                throw new AggregateAssertException(thenExceptions);
        }

        public override string ToString() => GetDescription().ToString();
    }
}