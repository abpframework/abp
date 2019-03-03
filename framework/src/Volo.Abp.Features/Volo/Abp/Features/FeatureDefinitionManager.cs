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
        protected Lazy<List<IFeatureDefinitionProvider>> Providers { get; }

        protected Lazy<IDictionary<string, FeatureDefinition>> FeatureDefinitions { get; }

        protected FeatureOptions Options { get; }

        private readonly IServiceProvider _serviceProvider;

        public FeatureDefinitionManager(
            IOptions<FeatureOptions> options,
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Options = options.Value;

            Providers = new Lazy<List<IFeatureDefinitionProvider>>(CreateFeatureProviders, true);
            FeatureDefinitions = new Lazy<IDictionary<string, FeatureDefinition>>(CreateFeatureDefinitions, true);
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
            return FeatureDefinitions.Value.Values.ToImmutableList();
        }

        public virtual FeatureDefinition GetOrNull(string name)
        {
            return FeatureDefinitions.Value.GetOrDefault(name);
        }

        protected virtual List<IFeatureDefinitionProvider> CreateFeatureProviders()
        {
            return Options
                .DefinitionProviders
                .Select(p => _serviceProvider.GetRequiredService(p) as IFeatureDefinitionProvider)
                .ToList();
        }

        protected virtual IDictionary<string, FeatureDefinition> CreateFeatureDefinitions()
        {
            var features = new Dictionary<string, FeatureDefinition>();

            foreach (var provider in Providers.Value)
            {
                provider.Define(new FeatureDefinitionContext(features));
            }

            return features;
        }
    }
}