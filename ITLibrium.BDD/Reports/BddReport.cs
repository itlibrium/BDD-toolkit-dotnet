using System;
using System.Collections.Generic;
using System.Reflection;
using ITLibrium.Bdd.Scenarios;

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

        public static void AddWriter(IBddReportWriter writer)
        {
            _writers.Add(writer);
        }

        public static IBddReport Create(string name)
        {
            var report = new BddReportImpl(name);
            _reports.Add(report);
            return report;
        }
        
        public static void AddScenarioResult(IBddScenarioResult result)
        {
            _defaultReport.AddScenarioResult(result);
        }
        
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
            foreach (IBddReport report in _reports)
            foreach (IBddReportWriter writer in _writers)
                writer.Write(report);
        }
    }
}