using System.IO;
using System.Threading.Tasks;

namespace ITLIBRIUM.BddToolkit.Docs
{
    public interface FilesProvider
    {
        Task CreateDirectory(string path);
        Task<Stream> CreateFile(string path);
    }
}