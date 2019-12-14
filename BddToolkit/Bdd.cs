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
        public static Feature Feature(string name, string description = null) => new Feature(name, description);
        
        [PublicAPI]
        public static Rule Rule(Feature feature, string name, string description = null) => 
            new Rule(feature, name, description);
        
        [PublicAPI]
        public static IFeatureAndRuleBuilder<TContext> Scenario<TContext>() 
            where TContext : class, new() => 
            new ScenarioBuilder<TContext>(new TContext(), _configuration.DocPublisher);
        
        [PublicAPI]
        public static IFeatureAndRuleBuilder<TContext> Scenario<TContext>(TContext context) 
            where TContext : class => 
            new ScenarioBuilder<TContext>(context, _configuration.DocPublisher);

        [PublicAPI]
        public static void Configure(Action<Configuration> setup) => setup(_configuration);

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