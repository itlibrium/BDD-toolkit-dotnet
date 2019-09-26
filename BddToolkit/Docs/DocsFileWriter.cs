using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ITLIBRIUM.BddToolkit.Docs
{
    public abstract class DocsFileWriter : DocsWriter
    {
        private readonly string _outputPath;
        private readonly Encoding _encoding;

        protected abstract string Extension { get; }

        protected DocsFileWriter() : this(null, Encoding.UTF8) { }
        
        protected DocsFileWriter(string outputPath) : this(outputPath, Encoding.UTF8) { }
        
        protected DocsFileWriter(Encoding encoding) : this(null, encoding) { }
        
        protected DocsFileWriter(string outputPath, Encoding encoding)
        {
            _outputPath = outputPath 
                ?? Environment.GetEnvironmentVariable("BddReportsOutputPath") 
                ?? AppContext.BaseDirectory;
            _encoding = encoding;
        }

        public Task Write(string value, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

//        public void Write(IBddReport report)
//        {
//            var fileName = Extension == null ? report.Name : $"{report.Name}.{Extension}";
//            using (var file = File.Create(Path.Combine(_outputPath, fileName)))
//            using (var writer = new StreamWriter(file, _encoding))
//                Write(report, writer);
//        }
//
//        protected abstract void Write(IBddReport report, StreamWriter writer);

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}