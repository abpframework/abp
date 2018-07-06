using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Volo.Abp.TenantManagement
{
    [DependsOn(typeof(AbpTenantManagementDomainModule))]
    [DependsOn(typeof(AbpTenantManagementApplicationContractsModule))]
    public class AbpTenantManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpTenantManagementApplicationAutoMapperProfile>(validate: true);
            });

            context.Services.AddAssemblyOf<AbpTenantManagementApplicationModule>();
        }
    }
}