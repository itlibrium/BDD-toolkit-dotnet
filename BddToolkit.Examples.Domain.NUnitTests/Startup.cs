using System.Threading;
using ITLIBRIUM.BddToolkit.Docs;
using JetBrains.Annotations;
using NUnit.Framework;

namespace ITLIBRIUM.BddToolkit.Examples
{
    [UsedImplicitly]
    [SetUpFixture]
    public class Startup
    {
        [OneTimeSetUp]
        public void Setup() => Bdd.Configure(configuration => configuration
            .Use(DocPublishers.GherkinFiles()));
        
        [OneTimeTearDown]
        public void PublishDocs() => Bdd.PublishDocs(CancellationToken.None);
    }
}