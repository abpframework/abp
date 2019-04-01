﻿using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;

namespace Volo.Abp.FeatureManagement
{
    public class DefaultValueFeatureManagementProvider : IFeatureManagementProvider, ISingletonDependency
    {
        public string Name => DefaultValueFeatureValueProvider.ProviderName;

        public Task<string> GetOrNullAsync(FeatureDefinition feature, string providerKey)
        {
            return Task.FromResult(feature.DefaultValue);
        }

        public Task SetAsync(FeatureDefinition feature, string value, string providerKey)
        {
            throw new AbpException($"Can not set default value of a feature. It is only possible while defining the feature in a {typeof(IFeatureDefinitionProvider)} implementation.");
        }

        public Task ClearAsync(FeatureDefinition feature, string providerKey)
        {
            throw new AbpException($"Can not clear default value of a feature. It is only possible while defining the feature in a {typeof(IFeatureDefinitionProvider)} implementation.");
        }
    }
}