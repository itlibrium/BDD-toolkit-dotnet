using System;
using System.Linq;
using ITLIBRIUM.BddToolkit.Syntax.Features;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Builders.Features
{
    public class FeatureBuilder
    {
        private readonly string _name;
        private string _description;
        private string[] _tags;

        public FeatureBuilder([NotNull] string name) => _name = name ?? throw new ArgumentNullException(nameof(name));

        public FeatureBuilder Description(string description)
        {
            _description = description;
            return this;
        }

        public FeatureBuilder Tags(params string[] tags)
        {
            _tags = tags;
            return this;
        }

        [PublicAPI]
        public Feature Build() => Feature.New(_name, _description, _tags ?? Enumerable.Empty<string>());

        public static implicit operator Feature(FeatureBuilder builder) => builder.Build();
    }
}