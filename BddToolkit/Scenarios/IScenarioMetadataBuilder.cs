using System;
using ITLIBRIUM.BddToolkit.Reports;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Scenarios
{
    public interface IScenarioMetadataBuilder
    {
        [PublicAPI]
        IScenarioMetadataBuilder TestedComponent(string testedComponent);
        
        [PublicAPI]
        IScenarioMetadataBuilder ReportTo(IBddReport report);
        
        [PublicAPI]
        IScenarioMetadataBuilder ExcludeFromReports();
        
        [PublicAPI]
        IGivenBuilder<TContext> Using<TContext>() where TContext : class, new();
        
        [PublicAPI]
        IGivenBuilder<TContext> Using<TContext>(TContext context);
    }
}