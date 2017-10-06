using System;
using System.Collections.Generic;
using ITLibrium.Bdd.Reports;

namespace ITLibrium.Bdd.Scenarios
{
    internal class FixtureBuilder : IFixtureBuilder
    {
        private string _testedComponent;
        
        private readonly string _title;

        private List<IBddReport> _reports;

        private bool _excludeFromReports;

        public FixtureBuilder(string testedComponent, string title, bool excludeFromReports, IBddReport report)
        {
            if (excludeFromReports && report != null)
                throw new InvalidOperationException();

            _testedComponent = testedComponent;
            _title = title;

            if (report != null)
                ReportTo(report);
        }

        public IFixtureBuilder TestedComponent(string testedComponent)
        {
            _testedComponent = testedComponent;
            return this;
        }

        public IFixtureBuilder ReportTo(IBddReport report)
        {
            if (_excludeFromReports)
                throw new InvalidOperationException();

            if (_reports == null)
                _reports = new List<IBddReport>();

            _reports.Add(report);
            return this;
        }

        public IFixtureBuilder ExcludeFromReports()
        {
            if (_reports != null && _reports.Count > 0)
                throw new InvalidOperationException();

            _excludeFromReports = true;
            return this;
        }

        public IGivenContinuationBuilder<TFixture> Given<TFixture>() where TFixture : class, new()
        {
            return new BddScenarioBuilder<TFixture>(_testedComponent, _title, new TFixture(), _excludeFromReports, _reports);
        }

        public IGivenContinuationBuilder<TFixture> Given<TFixture>(TFixture fixture)
        {
            return new BddScenarioBuilder<TFixture>(_testedComponent, _title, fixture, _excludeFromReports, _reports);
        }
    }
}