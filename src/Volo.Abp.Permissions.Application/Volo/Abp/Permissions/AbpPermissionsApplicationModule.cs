using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Permissions
{
    [DependsOn(typeof(AbpPermissionsDomainModule))]
    [DependsOn(typeof(AbpPermissionsApplicationContractsModule))]
    public class AbpPermissionsApplicationModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpPermissionsApplicationModule>();
        }
    }
}
