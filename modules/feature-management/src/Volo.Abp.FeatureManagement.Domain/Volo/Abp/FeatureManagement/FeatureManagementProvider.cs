using System.Threading.Tasks;
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

        public virtual bool Compatible(string providerName)
        {
            return providerName == Name;
        }

        public virtual async Task<string> GetOrNullAsync(FeatureDefinition feature, string providerKey)
        {
            return await Store.GetOrNullAsync(feature.Name, Name, await NormalizeProviderKeyAsync(providerKey));
        }

        public virtual async Task SetAsync(FeatureDefinition feature, string value, string providerKey)
        {
            await Store.SetAsync(feature.Name, value, Name, await NormalizeProviderKeyAsync(providerKey));
        }

        public virtual async Task ClearAsync(FeatureDefinition feature, string providerKey)
        {
            await Store.DeleteAsync(feature.Name, Name, await NormalizeProviderKeyAsync(providerKey));
        }

        protected virtual Task<string> NormalizeProviderKeyAsync(string providerKey)
        {
            return Task.FromResult(providerKey);
        }
    }
}
