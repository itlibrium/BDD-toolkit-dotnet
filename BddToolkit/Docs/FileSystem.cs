using System.IO;
using System.Threading.Tasks;

namespace ITLIBRIUM.BddToolkit.Docs
{
    public class FileSystem : FilesProvider
    {
        public Task CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
            return Task.CompletedTask;
        }

        public Task<Stream> CreateFile(string path) => Task.FromResult<Stream>(File.Create(path));
    }
}