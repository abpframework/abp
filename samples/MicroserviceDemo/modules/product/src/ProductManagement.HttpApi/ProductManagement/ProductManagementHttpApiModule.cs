using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace ProductManagement
{
    [DependsOn(
        typeof(ProductManagementApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class ProductManagementHttpApiModule : AbpModule
    {
        
    }
}
