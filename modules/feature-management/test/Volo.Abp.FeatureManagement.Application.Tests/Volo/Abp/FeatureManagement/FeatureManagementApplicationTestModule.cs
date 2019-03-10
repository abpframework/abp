using Volo.Abp.Modularity;

namespace Abp.FeatureManagement
{
    [DependsOn(
        typeof(FeatureManagementApplicationModule),
        typeof(FeatureManagementDomainTestModule)
        )]
    public class FeatureManagementApplicationTestModule : AbpModule
    {

    }
}
