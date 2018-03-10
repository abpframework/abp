using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Volo.Abp.Permissions
{
    [DependsOn(typeof(AbpDddApplicationModule))]
    [DependsOn(typeof(AbpPermissionsDomainSharedModule))]
    public class AbpPermissionsApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpPermissionsApplicationContractsModule>();
        }
    }
}
