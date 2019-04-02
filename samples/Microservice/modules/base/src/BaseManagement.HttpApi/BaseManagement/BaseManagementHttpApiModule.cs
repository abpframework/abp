using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace BaseManagement
{
    [DependsOn(
        typeof(BaseManagementApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class BaseManagementHttpApiModule : AbpModule
    {
        
    }
}
