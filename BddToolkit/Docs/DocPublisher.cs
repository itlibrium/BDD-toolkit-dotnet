using System;
using System.Threading;
using System.Threading.Tasks;
using ITLIBRIUM.BddToolkit.Syntax.Scenarios;
using ITLIBRIUM.BddToolkit.Tests;

namespace ITLIBRIUM.BddToolkit.Docs
{
    public interface DocPublisher : IDisposable
    {
        Task Append(in Scenario scenario, TestStatus testStatus, CancellationToken cancellationToken);
        Task Publish(CancellationToken cancellationToken);
    }
}