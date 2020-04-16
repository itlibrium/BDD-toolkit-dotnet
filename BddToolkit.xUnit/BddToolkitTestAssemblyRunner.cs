using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ITLIBRIUM.BddToolkit.xUnit
{
    public class BddToolkitTestAssemblyRunner : XunitTestAssemblyRunner
    {
        public BddToolkitTestAssemblyRunner(ITestAssembly testAssembly,
            IEnumerable<IXunitTestCase> testCases,
            IMessageSink diagnosticMessageSink,
            IMessageSink executionMessageSink,
            ITestFrameworkExecutionOptions executionOptions)
            : base(testAssembly, testCases, diagnosticMessageSink, executionMessageSink, executionOptions) { }

        protected override async Task AfterTestAssemblyStartingAsync()
        {
            await base.AfterTestAssemblyStartingAsync();

            var startupClasses = TestAssembly.Assembly
                .GetTypes(true)
                .Where(t =>
                {
                    var type = t.ToRuntimeType();
                    return type.IsClass &&
                           !type.IsAbstract &&
                           typeof(IBddToolkitStartup).IsAssignableFrom(type);
                })
                .Select(t => t.ToRuntimeType())
                .ToList();
            if (startupClasses.Count == 0)
                return;
            if (startupClasses.Count > 1)
                throw new InvalidOperationException(
                    $"{nameof(IBddToolkitStartup)} can not have more than one implementation");
            var startup = (IBddToolkitStartup) Activator.CreateInstance(startupClasses[0]);
            Bdd.Configure(startup.Setup);
        }

        protected override async Task BeforeTestAssemblyFinishedAsync()
        {
            await Bdd.PublishDocs();
            await base.BeforeTestAssemblyFinishedAsync();
        }
    }
}