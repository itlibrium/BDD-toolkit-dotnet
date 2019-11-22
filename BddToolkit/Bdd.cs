using System;
using ITLIBRIUM.BddToolkit.Builders;
using ITLIBRIUM.BddToolkit.Docs;
using ITLIBRIUM.BddToolkit.Syntax.Features;
using ITLIBRIUM.BddToolkit.Syntax.Rules;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit
{
    public static class Bdd
    {
        private static readonly Configuration _configuration;
        private static bool _isDisposed;
        
        static Bdd()
        {
            _configuration = Configuration.Default();
            AppDomain.CurrentDomain.ProcessExit += OnExit;
            AppDomain.CurrentDomain.DomainUnload += OnExit;
        }
        
        [PublicAPI]
        public static Feature Feature(string name) => new Feature(name, default);
        
        [PublicAPI]
        public static Rule Rule(Feature feature, string name) => new Rule(feature, name, default);
        
        [PublicAPI]
        public static IScenarioDescriptionBuilder<TContext> Scenario<TContext>() 
            where TContext : class, new() => 
            new ScenarioBuilder<TContext>(new TContext(), _configuration.DocPublisher);
        
        [PublicAPI]
        public static IScenarioDescriptionBuilder<TContext> Scenario<TContext>(TContext context) 
            where TContext : class => 
            new ScenarioBuilder<TContext>(context, _configuration.DocPublisher);

        private static void OnExit(object sender, EventArgs e)
        {
            lock (_configuration)
            {
                if(_isDisposed) return;
                _configuration.DocPublisher?.Dispose();
                _isDisposed = true;
            }
        }
        
        public class Configuration
        {
            [PublicAPI]
            public DocPublisher DocPublisher { get; set; }
        
            public static Configuration Default() => new Configuration(
                new NullDocPublisher());

            private Configuration([NotNull] DocPublisher resultPublisher)
            {
                DocPublisher = resultPublisher ?? throw new ArgumentNullException(nameof(resultPublisher));
            }
        }
    }
}