using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Humanizer;
using ITLIBRIUM.BddToolkit.Syntax.Scenarios;
using ITLIBRIUM.BddToolkit.Tests;

namespace ITLIBRIUM.BddToolkit.Docs
{
    internal class GherkinDocsFormatter : DocFormatter
    {
        public Task Write(Scenario scenario, TestStatus testStatus, DocsWriter writer, 
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private Scenario CreateScenario(string name)
        {
            //TODO: configurable "no action" text
//            var givenText = CreateSectionText("Given", _givenSteps, "no action");
//            var whenText = $"When {_whenStep.Name}".Humanize();
//            var thenText = CreateSectionText("Then", _thenSteps, "nothing happened");
//            return new Scenario(_name ?? name, _description, givenText, whenText, thenText);
            throw new NotImplementedException();
        }

        private static string CreateSectionText(string sectionName, IReadOnlyList<ScenarioStep> steps,
            string noActionText)
        {
            var builder = new StringBuilder();
            var actionsCount = steps.Count;
            if (actionsCount == 0)
            {
                builder.Append($"{sectionName} {noActionText}");
            }
            else
            {
                builder.Append($"{sectionName} {steps[0].Name}".Humanize());
                for (var i = 1; i < actionsCount; i++)
                {
                    builder.AppendLine();
                    builder.Append('\t');
                    builder.Append($"And {steps[i].Name}".Humanize());
                }
            }
            return builder.ToString();
        }

        public void Dispose()
        {
            //Nothing to dispose
        }
    }
}