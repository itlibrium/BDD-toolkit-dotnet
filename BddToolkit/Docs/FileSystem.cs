using System.IO;

namespace ITLIBRIUM.BddToolkit.Docs
{
    public class FileSystem : FilesProvider
    {
        public Stream Create(string path) => File.Create(path);
    }
}