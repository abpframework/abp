using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Modularity;

namespace Volo.Abp.MultiTenancy
{
    [DependsOn(typeof(AbpDddApplicationModule))]
    [DependsOn(typeof(AbpMultiTenancyDomainSharedModule))]
    public class AbpMultiTenancyApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<PermissionOptions>(options =>
            {
                options.DefinitionProviders.Add<AbpTenantManagementPermissionDefinitionProvider>();
            });

            services.AddAssemblyOf<AbpMultiTenancyApplicationContractsModule>();
        }
    }
}