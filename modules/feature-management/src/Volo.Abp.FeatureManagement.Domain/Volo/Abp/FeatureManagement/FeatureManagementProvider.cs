﻿using System.Threading.Tasks;
using Volo.Abp.Features;

namespace Volo.Abp.FeatureManagement
{
    public abstract class FeatureManagementProvider : IFeatureManagementProvider
    {
        public abstract string Name { get; }

        protected IFeatureManagementStore Store { get; }

        protected FeatureManagementProvider(IFeatureManagementStore store)
        {
            Store = store;
        }

        public async Task<string> GetOrNullAsync(FeatureDefinition feature, string providerKey)
        {
            return await Store.GetOrNullAsync(feature.Name, Name, NormalizeProviderKey(providerKey)).ConfigureAwait(false);
        }

        public virtual async Task SetAsync(FeatureDefinition feature, string value, string providerKey)
        {
            await Store.SetAsync(feature.Name, value, Name, NormalizeProviderKey(providerKey)).ConfigureAwait(false);
        }

        public virtual async Task ClearAsync(FeatureDefinition feature, string providerKey)
        {
            await Store.DeleteAsync(feature.Name, Name, NormalizeProviderKey(providerKey)).ConfigureAwait(false);
        }

        protected virtual string NormalizeProviderKey(string providerKey)
        {
            return providerKey;
        }
    }
}