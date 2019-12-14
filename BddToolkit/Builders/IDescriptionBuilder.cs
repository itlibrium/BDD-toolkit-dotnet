using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Builders
{
    public interface IDescriptionBuilder<TContext> : IGivenBuilder<TContext>
    {
        [PublicAPI]
        IGivenBuilder<TContext> Description(string description);
    }
}