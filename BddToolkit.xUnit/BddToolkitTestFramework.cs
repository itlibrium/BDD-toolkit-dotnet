using System.Reflection;
using JetBrains.Annotations;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ITLIBRIUM.BddToolkit.xUnit
{
    [PublicAPI]
    public class BddToolkitTestFramework : XunitTestFramework
    {
        public BddToolkitTestFramework(IMessageSink messageSink) : base(messageSink) { }

        protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName) =>
            new BddToolkitTestFrameworkExecutor(assemblyName, SourceInformationProvider, DiagnosticMessageSink);
    }
}