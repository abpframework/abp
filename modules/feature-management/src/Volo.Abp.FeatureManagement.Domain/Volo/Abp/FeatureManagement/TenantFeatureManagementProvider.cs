using Volo.Abp.Features;

namespace Volo.Abp.FeatureManagement
{
    public class TenantFeatureManagementProvider : FeatureManagementProvider
    {
        public override string Name => TenantFeatureValueProvider.ProviderName;

        public TenantFeatureManagementProvider(IFeatureManagementStore store) 
            : base(store)
        {
        }
    }
}