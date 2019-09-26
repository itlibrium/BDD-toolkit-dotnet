using System;
using System.Threading;
using System.Threading.Tasks;
using ITLIBRIUM.BddToolkit.Syntax.Scenarios;
using ITLIBRIUM.BddToolkit.Tests;

namespace ITLIBRIUM.BddToolkit.Docs
{
    public interface DocFormatter : IDisposable
    {
        Task Write(Scenario scenario, TestStatus testStatus, DocsWriter writer, CancellationToken cancellationToken);
    }
}