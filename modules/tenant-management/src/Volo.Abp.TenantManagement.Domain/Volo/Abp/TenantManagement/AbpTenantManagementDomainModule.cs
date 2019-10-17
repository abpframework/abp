using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Data;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.UI;

namespace Volo.Abp.TenantManagement
{
    [DependsOn(typeof(AbpMultiTenancyModule))]
    [DependsOn(typeof(AbpTenantManagementDomainSharedModule))]
    [DependsOn(typeof(AbpDataModule))]
    [DependsOn(typeof(AbpDddDomainModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    [DependsOn(typeof(AbpUiModule))] //TODO: It's not good to depend on the UI module. However, UserFriendlyException is inside it!
    public class AbpTenantManagementDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AbpTenantManagementDomainModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpTenantManagementDomainMappingProfile>(validate: true);
            });
        }
    }
}
