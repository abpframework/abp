using ProductManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace ProductManagement
{
    [DependsOn(
        typeof(ProductManagementEntityFrameworkCoreTestModule)
        )]
    public class ProductManagementDomainTestModule : AbpModule
    {
        
    }
}
