using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Permissions
{
    [DependsOn(typeof(AbpPermissionsModule))]
    [DependsOn(typeof(AbpDddModule))]
    [DependsOn(typeof(AbpPermissionsDomainSharedModule))]
    public class AbpPermissionsDomainModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpPermissionsDomainModule>();
        }
    }
}
