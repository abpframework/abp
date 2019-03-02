using System.Collections.Generic;
using System.Collections.Immutable;

namespace Volo.Abp.Features
{
    public class FeatureDefinitionContext : IFeatureDefinitionContext
    {
        protected Dictionary<string, FeatureDefinition> Features { get; }

        public FeatureDefinitionContext(Dictionary<string, FeatureDefinition> features)
        {
            Features = features;
        }

        public virtual FeatureDefinition GetOrNull(string name)
        {
            return Features.GetOrDefault(name);
        }

        public virtual IReadOnlyList<FeatureDefinition> GetAll()
        {
            return Features.Values.ToImmutableList();
        }

        public virtual void Add(params FeatureDefinition[] definitions)
        {
            if (definitions.IsNullOrEmpty())
            {
                return;
            }

            foreach (var definition in definitions)
            {
                Features[definition.Name] = definition;
            }
        }
    }
}