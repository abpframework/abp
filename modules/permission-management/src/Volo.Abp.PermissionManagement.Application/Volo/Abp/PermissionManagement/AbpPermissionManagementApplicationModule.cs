using Volo.Abp.Modularity;

namespace Volo.Abp.PermissionManagement
{
    [DependsOn(
        typeof(AbpPermissionManagementDomainModule), 
        typeof(AbpPermissionManagementApplicationContractsModule)
        )]
    public class AbpPermissionManagementApplicationModule : AbpModule
    {
        
    }
}
