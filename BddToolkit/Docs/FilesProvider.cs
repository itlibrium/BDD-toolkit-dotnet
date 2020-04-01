using System.IO;

namespace ITLIBRIUM.BddToolkit.Docs
{
    public interface FilesProvider
    {
        Stream Create(string path);
    }
}