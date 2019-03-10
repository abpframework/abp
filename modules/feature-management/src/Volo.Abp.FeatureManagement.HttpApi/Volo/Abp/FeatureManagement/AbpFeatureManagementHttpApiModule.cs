using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement
{
    [DependsOn(
        typeof(AbpFeatureManagementApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class AbpFeatureManagementHttpApiModule : AbpModule
    {
        
    }
}
