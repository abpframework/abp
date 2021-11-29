using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Features;

namespace Volo.Abp.FeatureManagement
{
    public static class DefaultValueFeatureManagerExtensions
    {
        public static Task<string> GetOrNullDefaultAsync(this IFeatureManager featureManager, [NotNull] string name, bool fallback = true)
        {
            return featureManager.GetOrNullAsync(name, DefaultValueFeatureValueProvider.ProviderName, null, fallback);
        }

        public static Task<List<FeatureNameValue>> GetAllDefaultAsync(this IFeatureManager featureManager, bool fallback = true)
        {
            return featureManager.GetAllAsync(DefaultValueFeatureValueProvider.ProviderName, null, fallback);
        }

        public static Task<FeatureNameValueWithGrantedProvider> GetOrNullWithProviderAsync(this IFeatureManager featureManager, [NotNull] string name, bool fallback = true)
        {
            return featureManager.GetOrNullWithProviderAsync(name, DefaultValueFeatureValueProvider.ProviderName, null, fallback);
        }

        public static Task<List<FeatureNameValueWithGrantedProvider>> GetAllWithProviderAsync(this IFeatureManager featureManager, bool fallback = true)
        {
            return featureManager.GetAllWithProviderAsync(DefaultValueFeatureValueProvider.ProviderName, null, fallback);
        }
    }
}
