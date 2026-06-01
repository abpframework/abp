using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement;

[DependsOn(
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpFeatureManagementDomainTestModule)
    )]
public class FeatureManagementApplicationTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<FeatureManagementOptions>(options =>
        {
            options.ProviderPolicies["test"] = FeatureManagementPermissions.ManageHostFeatures;
        });
    }
}
