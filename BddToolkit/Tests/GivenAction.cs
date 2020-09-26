using System.Threading.Tasks;

namespace ITLIBRIUM.BddToolkit.Tests
{
    internal delegate Task GivenAction<in TContext>(TContext context);
}