using System.Collections.Generic;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ITLIBRIUM.BddToolkit.xUnit
{
    public class BddToolkitTestFrameworkExecutor : XunitTestFrameworkExecutor
    {
        public BddToolkitTestFrameworkExecutor(AssemblyName assemblyName,
            ISourceInformationProvider sourceInformationProvider, IMessageSink diagnosticMessageSink)
            : base(assemblyName, sourceInformationProvider, diagnosticMessageSink) { }

        protected override async void RunTestCases(IEnumerable<IXunitTestCase> testCases,
            IMessageSink executionMessageSink, ITestFrameworkExecutionOptions executionOptions)
        {
            using var assemblyRunner = new BddToolkitTestAssemblyRunner(TestAssembly, testCases,
                DiagnosticMessageSink, executionMessageSink, executionOptions);
            await assemblyRunner.RunAsync();
        }
    }
}