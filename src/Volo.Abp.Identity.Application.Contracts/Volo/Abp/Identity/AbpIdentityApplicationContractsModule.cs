using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Modularity;
using Volo.Abp.Permissions;

namespace Volo.Abp.Identity
{
    [DependsOn(typeof(AbpIdentityDomainSharedModule))]
    [DependsOn(typeof(AbpAuthorizationModule))]
    [DependsOn(typeof(AbpDddApplicationModule))]
    [DependsOn(typeof(AbpPermissionsApplicationContractsModule))]
    public class AbpIdentityApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<PermissionOptions>(options =>
            {
                options.DefinitionProviders.Add<IdentityPermissionDefinitionProvider>();
            });

            services.AddAssemblyOf<AbpIdentityApplicationContractsModule>();
        }
    }
}