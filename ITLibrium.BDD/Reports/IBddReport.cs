using System.Collections.Generic;
using ITLibrium.Bdd.Scenarios;

namespace ITLibrium.Bdd.Reports
{
    public interface IBddReport
    {
        string Name { get; }
        IEnumerable<IBddScenarioResult> ScenarioResults { get; }
        
        void AddScenarioResult(IBddScenarioResult result);
    }
}