using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Features
{
    public class FeatureDefinitionManager : IFeatureDefinitionManager, ISingletonDependency
    {
        protected IDictionary<string, FeatureGroupDefinition> FeatureGroupDefinitions => _lazyFeatureGroupDefinitions.Value;
        private readonly Lazy<Dictionary<string, FeatureGroupDefinition>> _lazyFeatureGroupDefinitions;

        protected IDictionary<string, FeatureDefinition> FeatureDefinitions => _lazyFeatureDefinitions.Value;
        private readonly Lazy<Dictionary<string, FeatureDefinition>> _lazyFeatureDefinitions;

        protected AbpFeatureOptions Options { get; }

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public FeatureDefinitionManager(
            IOptions<AbpFeatureOptions> options,
            IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            Options = options.Value;

            _lazyFeatureDefinitions = new Lazy<Dictionary<string, FeatureDefinition>>(
                CreateFeatureDefinitions,
                isThreadSafe: true
            );

            _lazyFeatureGroupDefinitions = new Lazy<Dictionary<string, FeatureGroupDefinition>>(
                CreateFeatureGroupDefinitions,
                isThreadSafe:true
            );
        }

        public virtual FeatureDefinition Get(string name)
        {
            Check.NotNull(name, nameof(name));

            var feature = GetOrNull(name);

            if (feature == null)
            {
                throw new AbpException("Undefined feature: " + name);
            }

            return feature;
        }

        public virtual IReadOnlyList<FeatureDefinition> GetAll()
        {
            return FeatureDefinitions.Values.ToImmutableList();
        }

        public virtual FeatureDefinition GetOrNull(string name)
        {
            return FeatureDefinitions.GetOrDefault(name);
        }

        protected virtual Dictionary<string, FeatureDefinition> CreateFeatureDefinitions()
        {
            var features = new Dictionary<string, FeatureDefinition>();

            foreach (var groupDefinition in FeatureGroupDefinitions.Values)
            {
                foreach (var feature in groupDefinition.Features)
                {
                    AddFeatureToDictionaryRecursively(features, feature);
                }
            }

            return features;
        }

        protected virtual void AddFeatureToDictionaryRecursively(
            Dictionary<string, FeatureDefinition> features, 
            FeatureDefinition feature)
        {
            if (features.ContainsKey(feature.Name))
            {
                throw new AbpException("Duplicate feature name: " + feature.Name);
            }

            features[feature.Name] = feature;

            foreach (var child in feature.Children)
            {
                AddFeatureToDictionaryRecursively(features, child);
            }
        }

        protected virtual Dictionary<string, FeatureGroupDefinition> CreateFeatureGroupDefinitions()
        {
            var context = new FeatureDefinitionContext();

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var providers = Options
                    .DefinitionProviders
                    .Select(p => scope.ServiceProvider.GetRequiredService(p) as IFeatureDefinitionProvider)
                    .ToList();

                foreach (var provider in providers)
                {
                    provider.Define(context);
                }
            }

            return context.Groups;
        }
    }
}