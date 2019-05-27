using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement
{
    [DependsOn(
        typeof(FeatureManagementApplicationModule),
        typeof(FeatureManagementDomainTestModule)
        )]
    public class FeatureManagementApplicationTestModule : AbpModule
    {

    }
}
