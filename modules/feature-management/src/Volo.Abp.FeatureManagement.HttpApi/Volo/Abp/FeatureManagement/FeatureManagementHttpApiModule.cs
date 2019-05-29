using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Volo.Abp.FeatureManagement
{
    [DependsOn(
        typeof(FeatureManagementApplicationContractsModule),
        typeof(AspNetCoreMvcModule))]
    public class FeatureManagementHttpApiModule : AbpModule
    {
        
    }
}
