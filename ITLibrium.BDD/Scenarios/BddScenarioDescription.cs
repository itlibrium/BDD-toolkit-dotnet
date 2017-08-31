namespace ITLibrium.Bdd.Scenarios
{
    internal class BddScenarioDescription : IBddScenarioDescription
    {
        private readonly string _wholeText;

        public string Title { get; }
        public string Given { get; }
        public string When { get; }
        public string Then { get; }

        public BddScenarioDescription(string title, string given, string when, string then, string wholeText)
        {
            _wholeText = wholeText;
            Title = title;
            Given = given;
            When = when;
            Then = then;
        }

        public override string ToString() => _wholeText;

        public static implicit operator string(BddScenarioDescription description) => description.ToString();
    }
}