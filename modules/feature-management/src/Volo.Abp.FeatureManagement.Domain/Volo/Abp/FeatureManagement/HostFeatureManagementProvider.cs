using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.FeatureManagement
{
    public class HostFeatureManagementProvider : FeatureManagementProvider, ITransientDependency
    {
        public override string Name => HostFeatureValueProvider.ProviderName;

        protected ICurrentTenant CurrentTenant { get; }

        public HostFeatureManagementProvider(
            IFeatureManagementStore store,
            ICurrentTenant currentTenant)
            : base(store)
        {
            CurrentTenant = currentTenant;
        }

        public override async Task<string> GetOrNullAsync(FeatureDefinition feature, string providerKey)
        {
            if (IsHostSide(feature))
            {
                return await Store.GetOrNullAsync(feature.Name, Name, NormalizeProviderKey(providerKey));
            }

            return null;
        }

        public override async Task SetAsync(FeatureDefinition feature, string value, string providerKey)
        {
            if (IsHostSide(feature))
            {
                await Store.SetAsync(feature.Name, value, Name, NormalizeProviderKey(providerKey));
            }
        }

        public override async Task ClearAsync(FeatureDefinition feature, string providerKey)
        {
            if (IsHostSide(feature))
            {
                await Store.DeleteAsync(feature.Name, Name, NormalizeProviderKey(providerKey));
            }
        }

        protected override string NormalizeProviderKey(string providerKey)
        {
            return null;
        }

        //TODO: Should throw an ex when there is not in the host side?
        protected virtual bool IsHostSide(FeatureDefinition feature)
        {
            return feature.IsAvailableToHost && CurrentTenant.Id == null;
        }
    }
}
