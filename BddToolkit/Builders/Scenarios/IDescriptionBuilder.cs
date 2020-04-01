using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Builders.Scenarios
{
    public interface IDescriptionBuilder<TContext> : ITagsBuilder<TContext>
    {
        [PublicAPI]
        ITagsBuilder<TContext> Description(string description);
    }
}