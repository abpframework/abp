using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace ProductManagement
{
    [DependsOn(
        typeof(ProductManagementApplicationModule),
        typeof(ProductManagementDomainTestModule)
        )]
    public class ProductManagementApplicationTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAlwaysAllowAuthorization();
        }
    }
}
