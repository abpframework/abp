using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Permissions;
using Volo.Abp.Session;

namespace Volo.Abp.Identity
{
    [DependsOn(
        typeof(AbpIdentityDomainModule), 
        typeof(AbpIdentityApplicationContractsModule), 
        typeof(AbpAutoMapperModule),
        typeof(AbpSessionModule)
        )]
    public class AbpIdentityApplicationModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<PermissionManagementOptions>(options =>
            {
                options.ManagementProviders.Add<RolePermissionManagementProvider>();
            });

            services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpIdentityApplicationModuleAutoMapperProfile>();
            });

            services.AddAssemblyOf<AbpIdentityApplicationModule>();
        }
    }
}