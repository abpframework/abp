using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace Volo.Abp.Identity
{
    [DependsOn(
        typeof(AbpIdentityDomainModule), 
        typeof(AbpIdentityApplicationContractsModule), 
        typeof(AbpAutoMapperModule),
        typeof(AbpPermissionManagementApplicationModule)
        )]
    public class AbpIdentityApplicationModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpIdentityApplicationModuleAutoMapperProfile>();
            });

            services.AddAssemblyOf<AbpIdentityApplicationModule>();
        }
    }
}