using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.FeatureManagement
{
    public class TenantFeatureManagementProvider : FeatureManagementProvider, ITransientDependency
    {
        public override string Name => TenantFeatureValueProvider.ProviderName;

        protected ICurrentTenant CurrentTenant { get; }

        public TenantFeatureManagementProvider(
            IFeatureManagementStore store,
            ICurrentTenant currentTenant)
            : base(store)
        {
            CurrentTenant = currentTenant;
        }

        protected override string NormalizeProviderKey(string providerKey)
        {
            if (providerKey != null)
            {
                return providerKey;
            }

            return CurrentTenant.Id?.ToString();
        }
    }
}