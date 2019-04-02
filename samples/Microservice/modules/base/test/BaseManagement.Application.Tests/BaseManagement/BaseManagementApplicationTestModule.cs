using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace BaseManagement
{
    [DependsOn(
        typeof(BaseManagementApplicationModule),
        typeof(BaseManagementDomainTestModule)
        )]
    public class BaseManagementApplicationTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAlwaysAllowAuthorization();
        }
    }
}
