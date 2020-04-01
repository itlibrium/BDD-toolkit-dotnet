using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Builders.Scenarios
{
    public interface INameBuilder<TContext> : IDescriptionBuilder<TContext>
    {
        [PublicAPI]
        IDescriptionBuilder<TContext> Name(string name);
    }
}