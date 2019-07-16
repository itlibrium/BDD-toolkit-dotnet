namespace ITLIBRIUM.BddToolkit.Scenarios
{
    internal abstract class BddAction
    {
        public string Name { get; }

        protected BddAction(string name) => Name = name;
    }
}