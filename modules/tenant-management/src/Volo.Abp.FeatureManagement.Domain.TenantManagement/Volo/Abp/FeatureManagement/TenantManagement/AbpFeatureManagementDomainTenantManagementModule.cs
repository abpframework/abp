using Volo.Abp.Features;
using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement.TenantManagement
{
    public class AbpFeatureManagementDomainTenantManagementModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<FeatureManagementOptions>(options =>
            {
                options.Providers.Add<TenantFeatureManagementProvider>();

                options.ProviderPolicies[TenantFeatureValueProvider.ProviderName] = "AbpTenantManagement.Tenants.ManageFeatures";
            });
        }
    }
}
