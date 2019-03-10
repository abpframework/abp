using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Abp.FeatureManagement
{
    [DependsOn(
        typeof(FeatureManagementApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class FeatureManagementHttpApiModule : AbpModule
    {
        
    }
}
