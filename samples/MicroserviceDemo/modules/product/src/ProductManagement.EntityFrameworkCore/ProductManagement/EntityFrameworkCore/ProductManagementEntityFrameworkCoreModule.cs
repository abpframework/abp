using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace ProductManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(ProductManagementDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class ProductManagementEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<ProductManagementDbContext>(options =>
            {
                options.AddDefaultRepositories();
            });
        }
    }
}