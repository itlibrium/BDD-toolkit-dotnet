using System;
using System.Collections.Generic;
using System.Reflection;
using ITLibrium.Bdd.Scenarios;
using JetBrains.Annotations;

namespace ITLibrium.Bdd.Reports
{
    public static class BddReport
    {
        private static readonly IBddReport _defaultReport;

        private static readonly List<IBddReport> _reports = new List<IBddReport>();

        private static readonly List<IBddReportWriter> _writers = new List<IBddReportWriter>();
        
        private static bool _isFinished;
        
        static BddReport()
        {
            _defaultReport = new BddReportImpl($"BDD Report for {Assembly.GetCallingAssembly().GetName().Name}");
            _reports.Add(_defaultReport);
            
            AppDomain.CurrentDomain.ProcessExit += OnExit;
            AppDomain.CurrentDomain.DomainUnload += OnExit;
        }

        [PublicAPI]
        public static void AddWriter(IBddReportWriter writer) => _writers.Add(writer);

        [PublicAPI]
        public static IBddReport Create(string name)
        {
            var report = new BddReportImpl(name);
            _reports.Add(report);
            return report;
        }
        
        [PublicAPI]
        public static void AddScenarioResult(IBddScenarioResult result) => _defaultReport.AddScenarioResult(result);

        private static void OnExit(object sender, EventArgs args)
        {
            lock (_reports)
            {
                if (_isFinished)
                    return;

                WriteReports();

                _isFinished = true;
            }
        }

        private static void WriteReports()
        {
            foreach (var report in _reports)
            foreach (var writer in _writers)
                writer.Write(report);
        }
    }
}