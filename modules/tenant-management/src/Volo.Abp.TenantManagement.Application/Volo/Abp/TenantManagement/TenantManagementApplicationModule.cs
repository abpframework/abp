using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Volo.Abp.TenantManagement
{
    [DependsOn(typeof(TenantManagementDomainModule))]
    [DependsOn(typeof(TenantManagementApplicationContractsModule))]
    [DependsOn(typeof(AutoMapperModule))]
    public class TenantManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpTenantManagementApplicationAutoMapperProfile>(validate: true);
            });
        }
    }
}