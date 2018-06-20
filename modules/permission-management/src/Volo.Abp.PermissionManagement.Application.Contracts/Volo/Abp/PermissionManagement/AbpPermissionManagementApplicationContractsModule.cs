using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Volo.Abp.PermissionManagement
{
    [DependsOn(typeof(AbpDddApplicationModule))]
    [DependsOn(typeof(AbpPermissionManagementDomainSharedModule))]
    public class AbpPermissionManagementApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpPermissionManagementApplicationContractsModule>();
        }
    }
}
