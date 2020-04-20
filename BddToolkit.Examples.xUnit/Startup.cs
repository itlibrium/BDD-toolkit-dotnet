using ITLIBRIUM.BddToolkit.Docs;
using ITLIBRIUM.BddToolkit.xUnit;
using JetBrains.Annotations;

[assembly: UseBddToolkitTestFramework]

namespace ITLIBRIUM.BddToolkit.Examples
{
    [UsedImplicitly]
    public class Startup : IBddToolkitStartup
    {
        public void Setup(Configuration configuration) => configuration
            .Use(DocPublishers.GherkinFiles());
    }
}