using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement;

[DependsOn(
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpFeatureManagementDomainTestModule)
    )]
public class FeatureManagementApplicationTestModule : AbpModule
{

}
