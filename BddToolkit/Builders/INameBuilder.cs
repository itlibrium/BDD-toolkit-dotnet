using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Builders
{
    public interface INameBuilder<TContext> : IDescriptionBuilder<TContext>
    {
        [PublicAPI]
        IDescriptionBuilder<TContext> Name(string name);
    }
}