using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Data;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.TenantManagement
{
    [DependsOn(typeof(AbpMultiTenancyModule))]
    [DependsOn(typeof(AbpTenantManagementDomainSharedModule))]
    [DependsOn(typeof(AbpDataModule))]
    [DependsOn(typeof(AbpDddDomainModule))]
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class AbpTenantManagementDomainModule : AbpModule
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AbpTenantManagementDomainModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<AbpTenantManagementDomainMappingProfile>(validate: true);
            });

            Configure<AbpDistributedEntityEventOptions>(options =>
            {
                options.EtoMappings.Add<Tenant, TenantEto>();
            });
        }

        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            OneTimeRunner.Run(() =>
            {
                ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                    TenantManagementModuleExtensionConsts.ModuleName,
                    TenantManagementModuleExtensionConsts.EntityNames.Tenant,
                    typeof(Tenant)
                );
            });
        }
    }
}
