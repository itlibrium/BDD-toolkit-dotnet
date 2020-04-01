using System;
using ITLIBRIUM.BddToolkit.Syntax.Features;
using ITLIBRIUM.BddToolkit.Syntax.Rules;
using JetBrains.Annotations;

namespace ITLIBRIUM.BddToolkit.Builders.Rules
{
    public class RuleBuilder
    {
        private readonly string _name;
        private string _description;
        private Feature _feature = Syntax.Features.Feature.Empty();

        public RuleBuilder([NotNull] string name) => _name = name ?? throw new ArgumentNullException(nameof(name));

        public RuleBuilder Description(string description)
        {
            _description = description;
            return this;
        }

        public RuleBuilder Feature(in Feature feature)
        {
            _feature = feature;
            return this;
        }

        [PublicAPI]
        public Rule Build() => Rule.New(_feature, _name, _description);

        public static implicit operator Rule(RuleBuilder builder) => builder.Build();
    }
}