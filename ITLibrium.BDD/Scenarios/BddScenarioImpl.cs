using System;
using System.Collections.Generic;
using System.Text;
using Humanizer;

namespace ITLibrium.Bdd.Scenarios
{
    internal class BddScenarioImpl<TFixture> : IBddScenario
    {
        private readonly string _title;

        private readonly TFixture _fixture;

        private readonly IReadOnlyList<GivenAction<TFixture>> _givenActions;
        private readonly WhenAction<TFixture> _whenAction;
        private readonly IReadOnlyList<ThenAction<TFixture>> _thenActions;

        public BddScenarioImpl(string title, TFixture fixture, IReadOnlyList<GivenAction<TFixture>> givenActions, WhenAction<TFixture> whenAction, IReadOnlyList<ThenAction<TFixture>> thenActions)
        {
            _title = title;
            _fixture = fixture;
            _givenActions = givenActions;
            _whenAction = whenAction;
            _thenActions = thenActions;
        }

        public IBddScenarioDescription GetDescription()
        {
            string givenText = CreateSectionText("Given", _givenActions);
            string whenText = $"When {_whenAction.Name}".Humanize();
            string thenText = CreateSectionText("Then", _thenActions);

            var builder = new StringBuilder();
            builder.AppendLine(_title);
            builder.AppendLine(givenText);
            builder.AppendLine(whenText);
            builder.AppendLine(thenText);

            return new BddScenarioDescription(_title, givenText, whenText, thenText, builder.ToString());
        }

        private static string CreateSectionText(string sectionName, IReadOnlyList<BddAction> actions)
        {
            var builder = new StringBuilder();

            builder.Append($"{sectionName} {actions[0].Name}".Humanize());

            int count = actions.Count;
            for (int i = 1; i < count; i++)
            {
                builder.AppendLine();
                builder.Append('\t');
                builder.Append($"And {actions[i].Name}".Humanize());
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

            if (thenExceptions != null)
                throw new AggregateAssertException(thenExceptions);
        }

        public override string ToString() => GetDescription().ToString();
    }
}