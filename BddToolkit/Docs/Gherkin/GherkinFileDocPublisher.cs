using System;
using System.Threading;
using System.Threading.Tasks;
using ITLIBRIUM.BddToolkit.Syntax.Scenarios;
using ITLIBRIUM.BddToolkit.Tests;

namespace ITLIBRIUM.BddToolkit.Docs.Gherkin
{
    public class GherkinFileDocPublisher : DocPublisher
    {
        private readonly GherkinFormatter _formatter;

        public Task Append(Scenario scenario, TestStatus testStatus, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Publish(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}