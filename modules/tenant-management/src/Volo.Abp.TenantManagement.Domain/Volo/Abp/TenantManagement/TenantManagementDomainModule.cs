using Volo.Abp.AutoMapper;
using Volo.Abp.Data;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.UI;

namespace Volo.Abp.TenantManagement
{
    [DependsOn(typeof(MultiTenancyModule))]
    [DependsOn(typeof(TenantManagementDomainSharedModule))]
    [DependsOn(typeof(DataModule))]
    [DependsOn(typeof(DddDomainModule))]
    [DependsOn(typeof(AutoMapperModule))]
    [DependsOn(typeof(UiModule))] //TODO: It's not good to depend on the UI module. However, UserFriendlyException is inside it!
    public class TenantManagementDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpTenantManagementDomainMappingProfile>(validate: true);
            });
        }
    }
}
