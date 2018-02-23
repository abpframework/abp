using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Modularity;

namespace Volo.Abp.Permissions
{
    [DependsOn(typeof(AbpDddModule))]
    [DependsOn(typeof(AbpPermissionsDomainSharedModule))]
    public class AbpPermissionsApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<PermissionOptions>(options =>
            {
                options.DefinitionProviders.Add<PermissionsPermissionDefinitionProvider>();
            });

            services.AddAssemblyOf<AbpPermissionsApplicationContractsModule>();
        }
    }
}
