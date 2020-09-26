using System.Threading.Tasks;

namespace ITLIBRIUM.BddToolkit.Tests
{
    internal delegate Task ThenAction<in TContext>(TContext context);
}