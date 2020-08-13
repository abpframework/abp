using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;

namespace Volo.Abp.TenantManagement
{
    [DependsOn(
        typeof(AbpDddApplicationModule),
        typeof(AbpTenantManagementDomainSharedModule))]
    public class AbpTenantManagementApplicationContractsModule : AbpModule
    {
        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToApi(
                    TenantManagementModuleExtensionConsts.ModuleName,
                    TenantManagementModuleExtensionConsts.EntityNames.Tenant,
                    getApiTypes: new[] { typeof(TenantDto) },
                    createApiTypes: new[] { typeof(TenantCreateDto) },
                    updateApiTypes: new[] { typeof(TenantUpdateDto) }
                );
        }
    }
}