using System;
using JetBrains.Annotations;
using Xunit.Sdk;

namespace ITLIBRIUM.BddToolkit.xUnit
{
    [TestFrameworkDiscoverer("Xunit.Sdk.TestFrameworkTypeDiscoverer", "xunit.execution.{Platform}")]
    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class UseBddToolkitTestFramework : Attribute, ITestFrameworkAttribute
    {
        [UsedImplicitly]
        public UseBddToolkitTestFramework(string typeName = "ITLIBRIUM.BddToolkit.xUnit.BddToolkitTestFramework",
            string assemblyName = "ITLIBRIUM.BddToolkit.xUnit") { }
    }
}