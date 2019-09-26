using System;
using System.Threading;
using System.Threading.Tasks;

namespace ITLIBRIUM.BddToolkit.Docs
{
    public interface DocsWriter : IDisposable
    {
        Task Write(string value, CancellationToken cancellationToken);
    }
}