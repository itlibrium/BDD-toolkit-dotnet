using System.IO;
using System.Linq;
using System.Text;
using ITLibrium.Bdd.Scenarios;

namespace ITLibrium.Bdd.Reports
{
    public class BddReportTextWriter : BddReportFileWriter
    {
        protected override string Extension => "txt";

        public BddReportTextWriter() { }

        public BddReportTextWriter(string outputPath) : base(outputPath) { }

        public BddReportTextWriter(Encoding encoding) : base(encoding) { }

        public BddReportTextWriter(string outputPath, Encoding encoding) : base(outputPath, encoding) { }

        protected override void Write(IBddReport report, StreamWriter writer)
        {
            string reportName = report.Name;
            if (reportName != null)
            {
                writer.WriteLine(reportName);
                writer.WriteLine();
            }
                    
            foreach (IGrouping<string, IBddScenarioResult> grouping in report.ScenarioResults.GroupBy(r => r.Description.TestedComponent))
            {
                if (grouping.Key == null)
                {
                    writer.WriteLine("Tests not assigned to a specific component");
                    writer.WriteLine();
                }
                else
                {
                    writer.Write("Tests for: ");
                    writer.WriteLine(grouping.Key);
                    writer.WriteLine();
                }

                foreach (IBddScenarioResult result in grouping)
                {
                    writer.Write(result.Description);
                    writer.WriteLine();
                    writer.Write("Result: ");
                    writer.WriteLine(result.TestPassed ? "passed" : "failed");
                    writer.WriteLine();
                }
            }
        }
    }
}