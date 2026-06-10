using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Features;

namespace Volo.Abp.FeatureManagement;

public static class ConfigurationValueFeatureManagerExtensions
{
    public static Task<string> GetOrNullConfigurationAsync(this IFeatureManager featureManager, [NotNull] string name, bool fallback = true)
    {
        return featureManager.GetOrNullAsync(name, ConfigurationFeatureValueProvider.ProviderName, null, fallback);
    }

    public static Task<List<FeatureNameValue>> GetAllConfigurationAsync(this IFeatureManager featureManager, bool fallback = true)
    {
        return featureManager.GetAllAsync(ConfigurationFeatureValueProvider.ProviderName, null, fallback);
    }
}
