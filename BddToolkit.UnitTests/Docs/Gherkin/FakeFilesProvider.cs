using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ITLIBRIUM.BddToolkit.Docs.Gherkin
{
    public class FakeFilesProvider : FilesProvider
    {
        private readonly List<string> _streamsContent = new List<string>();
        public IReadOnlyList<string> StreamsContent => _streamsContent;

        public Task CreateDirectory(string path) => Task.CompletedTask;

        public Task<Stream> CreateFile(string path) => Task.FromResult<Stream>(new StreamWrapper(this));

        private void AddStreamContent(string streamContent) => _streamsContent.Add(streamContent);

        private class StreamWrapper : Stream
        {
            private readonly MemoryStream _stream = new MemoryStream();
            private readonly FakeFilesProvider _filesProvider;

            public StreamWrapper(FakeFilesProvider filesProvider) => _filesProvider = filesProvider;

            public override bool CanRead => _stream.CanRead;
            public override bool CanSeek => _stream.CanSeek;
            public override bool CanWrite => _stream.CanWrite;
            public override long Length => _stream.Length;

            public override long Position
            {
                get => _stream.Position;
                set => _stream.Position = value;
            }

            public override void Flush() => _stream.Flush();

            public override int Read(byte[] buffer, int offset, int count) => _stream.Read(buffer, offset, count);

            public override long Seek(long offset, SeekOrigin origin) => _stream.Seek(offset, origin);

            public override void SetLength(long value) => _stream.SetLength(value);

            public override void Write(byte[] buffer, int offset, int count) => _stream.Write(buffer, offset, count);

            protected override void Dispose(bool disposing)
            {
                var streamContent = ReadStreamContent(_stream);
                _filesProvider.AddStreamContent(streamContent);
                _stream.Dispose();
                base.Dispose(disposing);
            }

            private static string ReadStreamContent(Stream stream)
            {
                stream.Position = 0;
                using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
                return reader.ReadToEnd();
            }
        }
    }
}