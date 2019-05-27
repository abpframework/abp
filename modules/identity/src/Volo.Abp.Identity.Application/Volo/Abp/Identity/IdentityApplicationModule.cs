using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace Volo.Abp.Identity
{
    [DependsOn(
        typeof(IdentityDomainModule), 
        typeof(IdentityApplicationContractsModule), 
        typeof(AutoMapperModule),
        typeof(PermissionManagementApplicationModule)
        )]
    public class IdentityApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpIdentityApplicationModuleAutoMapperProfile>();
            });
        }
    }
}