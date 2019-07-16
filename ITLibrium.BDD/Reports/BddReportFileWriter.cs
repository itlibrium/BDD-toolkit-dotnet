using System;
using System.IO;
using System.Text;

namespace ITLibrium.Bdd.Reports
{
    public abstract class BddReportFileWriter : IBddReportWriter
    {
        private readonly string _outputPath;

        private readonly Encoding _encoding;

        protected abstract string Extension { get; }

        protected BddReportFileWriter() : this(null, Encoding.UTF8) { }
        
        protected BddReportFileWriter(string outputPath) : this(outputPath, Encoding.UTF8) { }
        
        protected BddReportFileWriter(Encoding encoding) : this(null, encoding) { }
        
        protected BddReportFileWriter(string outputPath, Encoding encoding)
        {
            _outputPath = outputPath 
                ?? Environment.GetEnvironmentVariable("BddReportsOutputPath") 
                ?? AppContext.BaseDirectory;
            _encoding = encoding;
        }

        public void Write(IBddReport report)
        {
            var fileName = Extension == null ? report.Name : $"{report.Name}.{Extension}";
            using (var file = File.Create(Path.Combine(_outputPath, fileName)))
            using (var writer = new StreamWriter(file, _encoding))
                Write(report, writer);
        }

        protected abstract void Write(IBddReport report, StreamWriter writer);
    }
}