using System;
using ITLIBRIUM.BddToolkit.Docs;

namespace ITLIBRIUM.BddToolkit.Configuration
{
    public class ConfigValues
    {
        public DocPublisher DocPublisher { get; }
        
        public static ConfigValues Default() => new ConfigValues(DocPublisher.Gherkin);

        private ConfigValues(DocPublisher resultPublisher)
        {
            DocPublisher = resultPublisher;
            AppDomain.CurrentDomain.ProcessExit += OnExit;
            AppDomain.CurrentDomain.DomainUnload += OnExit;
        }

        private void OnExit(object sender, EventArgs e) => DocPublisher.Dispose();
    }
}