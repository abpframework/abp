using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Volo.Abp.TenantManagement
{
    [DependsOn(typeof(AbpTenantManagementDomainModule))]
    [DependsOn(typeof(AbpTenantManagementApplicationContractsModule))]
    public class AbpTenantManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpTenantManagementApplicationAutoMapperProfile>(validate: true);
            });

            services.AddAssemblyOf<AbpTenantManagementApplicationModule>();
        }
    }
}