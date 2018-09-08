using Volo.Abp.Modularity;

namespace Volo.Abp.PermissionManagement
{
    [DependsOn(typeof(AbpPermissionManagementDomainModule))]
    [DependsOn(typeof(AbpPermissionManagementApplicationContractsModule))]
    public class AbpPermissionManagementApplicationModule : AbpModule
    {
        
    }
}
