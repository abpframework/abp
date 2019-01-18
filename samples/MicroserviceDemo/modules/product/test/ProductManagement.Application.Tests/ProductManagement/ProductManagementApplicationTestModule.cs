using Volo.Abp.Modularity;

namespace ProductManagement
{
    [DependsOn(
        typeof(ProductManagementApplicationModule),
        typeof(ProductManagementDomainTestModule)
        )]
    public class ProductManagementApplicationTestModule : AbpModule
    {

    }
}
