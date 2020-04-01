using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Builders.Scenarios
{
    public interface ITagsBuilder<TContext> : IGivenBuilder<TContext>
    {
        [PublicAPI]
        IGivenBuilder<TContext> Tags(params string[] tags);
    }
}