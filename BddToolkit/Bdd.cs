using System;
using System.Threading;
using ITLIBRIUM.BddToolkit.Builders.Features;
using ITLIBRIUM.BddToolkit.Builders.Rules;
using ITLIBRIUM.BddToolkit.Builders.Scenarios;
using ITLIBRIUM.BddToolkit.Syntax.Features;
using ITLIBRIUM.BddToolkit.Syntax.Rules;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit
{
    public static class Bdd
    {
        private static readonly Configuration Configuration;
        private static bool _isDisposed;

        static Bdd()
        {
            Configuration = Configuration.Default();
            AppDomain.CurrentDomain.ProcessExit += OnExit;
            AppDomain.CurrentDomain.DomainUnload += OnExit;
        }

        [PublicAPI]
        public static FeatureBuilder Feature([NotNull] string name) => new FeatureBuilder(name);
        
        [PublicAPI]
        [Obsolete("Use overload with name only and add description through fluent api")]
        public static FeatureBuilder Feature([NotNull] string name, string description) => new FeatureBuilder(name)
            .Description(description);
        
        [PublicAPI]
        public static RuleBuilder Rule([NotNull] string name) => new RuleBuilder(name);

        [PublicAPI]
        [Obsolete("Use overload with name only and add description and feature through fluent api")]
        public static Rule Rule(in Feature feature, string name, string description = null) =>
            Syntax.Rules.Rule.New(feature, name, description);

        [PublicAPI]
        public static IFeatureAndRuleBuilder<TContext> Scenario<TContext>()
            where TContext : class, new() =>
            new ScenarioBuilder<TContext>(new TContext(), Configuration.DocPublisher);

        [PublicAPI]
        public static IFeatureAndRuleBuilder<TContext> Scenario<TContext>(TContext context)
            where TContext : class =>
            new ScenarioBuilder<TContext>(context, Configuration.DocPublisher);

        [PublicAPI]
        public static void Configure(Action<Configuration> setup) => setup(Configuration);

        private static void OnExit(object sender, EventArgs e)
        {
            lock (Configuration)
            {
                if (_isDisposed) return;
                Configuration.DocPublisher.Publish(CancellationToken.None).Wait();
                Configuration.DocPublisher.Dispose();
                _isDisposed = true;
            }
        }
    }
}