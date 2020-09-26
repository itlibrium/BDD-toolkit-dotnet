using System.Threading.Tasks;

namespace ITLIBRIUM.BddToolkit.Tests
{
    public delegate Task WhenAction<in TContext>(TContext context);
}