using System;
using ITLIBRIUM.BddToolkit.Docs;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit
{
    public class Configuration
    {
        internal DocPublisher DocPublisher { get; private set; }

        public static Configuration Default() => new Configuration(
            new NullDocPublisher());

        private Configuration([NotNull] DocPublisher resultPublisher)
        {
            DocPublisher = resultPublisher ?? throw new ArgumentNullException(nameof(resultPublisher));
        }

        [PublicAPI]
        public Configuration Use([NotNull] DocPublisher docPublisher)
        {
            DocPublisher = docPublisher ?? throw new ArgumentNullException(nameof(docPublisher));
            return this;
        }
    }
}