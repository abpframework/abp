using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement
{
    [DependsOn(
        typeof(FeatureManagementApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class FeatureManagementHttpApiModule : AbpModule
    {
        
    }
}
