using System.Collections.Generic;
using ITLIBRIUM.BddToolkit.Scenarios;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Reports
{
    public interface IBddReport
    {
        string Name { get; }
        IEnumerable<IBddScenarioResult> ScenarioResults { get; }
        
        [PublicAPI]
        void AddScenarioResult(IBddScenarioResult result);
    }
}