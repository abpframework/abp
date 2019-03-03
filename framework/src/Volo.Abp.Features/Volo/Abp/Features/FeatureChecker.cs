using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Features
{
    public class FeatureChecker : IFeatureChecker, ITransientDependency
    {
        protected IFeatureDefinitionManager FeatureDefinitionManager { get; }
        protected Lazy<List<IFeatureValueProvider>> Providers { get; }
        protected FeatureOptions Options { get; }

        public FeatureChecker(
            IOptions<FeatureOptions> options,
            IServiceProvider serviceProvider,
            IFeatureDefinitionManager featureDefinitionManager)
        {
            FeatureDefinitionManager = featureDefinitionManager;

            Options = options.Value;

            Providers = new Lazy<List<IFeatureValueProvider>>(
                () => Options
                    .ValueProviders
                    .Select(type => serviceProvider.GetRequiredService(type) as IFeatureValueProvider)
                    .ToList(),
                true
            );
        }

        public virtual async Task<string> GetOrNullAsync(string name)
        {
            var featureDefinition = FeatureDefinitionManager.Get(name);
            var providers = Enumerable
                .Reverse(Providers.Value);

            if (featureDefinition.AllowedProviders.Any())
            {
                providers = providers.Where(p => featureDefinition.AllowedProviders.Contains(p.Name));
            }

            return await GetOrNullValueFromProvidersAsync(providers, featureDefinition);
        }

        public async Task<bool> IsEnabledAsync(string name)
        {
            var value = await GetOrNullAsync(name);
            if (value == null)
            {
                return false;
            }

            try
            {
                return bool.Parse(value);
            }
            catch (Exception ex)
            {
                throw new AbpException(
                    $"The value '{value}' for the feature '{name}' should be a boolean, but was not!",
                    ex
                );
            }
        }

        protected virtual async Task<string> GetOrNullValueFromProvidersAsync(
            IEnumerable<IFeatureValueProvider> providers,
            FeatureDefinition feature)
        {
            foreach (var provider in providers)
            {
                var value = await provider.GetOrNullAsync(feature);
                if (value != null)
                {
                    return value;
                }
            }

            return null;
        }
    }
}