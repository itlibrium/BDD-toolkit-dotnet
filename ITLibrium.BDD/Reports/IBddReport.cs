using System.Collections.Generic;
using ITLibrium.Bdd.Scenarios;
using JetBrains.Annotations;

namespace ITLibrium.Bdd.Reports
{
    public interface IBddReport
    {
        string Name { get; }
        IEnumerable<IBddScenarioResult> ScenarioResults { get; }
        
        [PublicAPI]
        void AddScenarioResult(IBddScenarioResult result);
    }
}