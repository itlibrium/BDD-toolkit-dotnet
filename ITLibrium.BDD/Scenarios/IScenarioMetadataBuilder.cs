using System;
using ITLibrium.Bdd.Reports;
using JetBrains.Annotations;

namespace ITLibrium.Bdd.Scenarios
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
        
        [Obsolete("Use 'Using' method instead")]
        IGivenContinuationBuilder<TContext> Given<TContext>() where TContext : class, new();
        
        [Obsolete("Use 'Using' method instead")]
        IGivenContinuationBuilder<TContext> Given<TContext>(TContext fixture);
    }
}