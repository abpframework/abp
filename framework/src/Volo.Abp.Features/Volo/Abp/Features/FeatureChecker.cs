using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Volo.Abp.Features;

public class FeatureChecker : FeatureCheckerBase
{
    protected AbpFeatureOptions Options { get; }
    protected IServiceProvider ServiceProvider { get; }
    protected IFeatureDefinitionManager FeatureDefinitionManager { get; }
    protected IFeatureValueProviderManager FeatureValueProviderManager { get; }

    public FeatureChecker(
        IOptions<AbpFeatureOptions> options,
        IServiceProvider serviceProvider,
        IFeatureDefinitionManager featureDefinitionManager,
        IFeatureValueProviderManager featureValueProviderManager)
    {
        ServiceProvider = serviceProvider;
        FeatureDefinitionManager = featureDefinitionManager;
        FeatureValueProviderManager = featureValueProviderManager;

        Options = options.Value;
    }

    public override async Task<string?> GetOrNullAsync(string name)
    {
        var featureDefinition = await FeatureDefinitionManager.GetAsync(name);
        var providers = FeatureValueProviderManager.ValueProviders
            .Reverse();

        if (featureDefinition.AllowedProviders.Any())
        {
            providers = providers.Where(p => featureDefinition.AllowedProviders.Contains(p.Name));
        }

        return await GetOrNullValueFromProvidersAsync(providers, featureDefinition);
    }

    protected virtual async Task<string?> GetOrNullValueFromProvidersAsync(
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
