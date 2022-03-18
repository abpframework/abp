using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement;

[DependsOn(
    typeof(AbpFeatureManagementDomainModule),
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(AbpDddApplicationModule)
    )]
public class AbpFeatureManagementApplicationModule : AbpModule
{

}
