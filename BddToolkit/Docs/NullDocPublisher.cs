using System.Threading;
using System.Threading.Tasks;
using ITLIBRIUM.BddToolkit.Syntax.Scenarios;
using ITLIBRIUM.BddToolkit.Tests;

namespace ITLIBRIUM.BddToolkit.Docs
{
    public class NullDocPublisher : DocPublisher
    {
        public Task Append(Scenario scenario, TestStatus testStatus, CancellationToken cancellationToken) => 
            Task.CompletedTask;

        public Task Publish(CancellationToken cancellationToken) => Task.CompletedTask;

        public void Dispose()
        {
            //nothing to dispose
        }
    }
}