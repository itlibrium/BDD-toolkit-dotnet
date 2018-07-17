using System;
using ITLibrium.Bdd.Reports;

namespace ITLibrium.Bdd.Scenarios
{
    public interface IScenarioMetadataBuilder
    {
        IScenarioMetadataBuilder TestedComponent(string testedComponent);
        
        IScenarioMetadataBuilder ReportTo(IBddReport report);
        IScenarioMetadataBuilder ExcludeFromReports();
        
        IGivenBuilder<TContext> Using<TContext>() where TContext : class, new();
        IGivenBuilder<TContext> Using<TContext>(TContext context);
        
        [Obsolete("Use 'Using' method instead")]
        IGivenContinuationBuilder<TContext> Given<TContext>() where TContext : class, new();
        [Obsolete("Use 'Using' method instead")]
        IGivenContinuationBuilder<TContext> Given<TContext>(TContext fixture);
    }
}