using System.Threading.Tasks;

namespace ITLIBRIUM.BddToolkit.Tests
{
    internal delegate Task ExceptionCheck<in TContext>(TContext context, Result whenActionResult);
}