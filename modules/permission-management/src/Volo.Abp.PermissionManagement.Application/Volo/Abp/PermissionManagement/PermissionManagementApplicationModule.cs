using Volo.Abp.Modularity;

namespace Volo.Abp.PermissionManagement
{
    [DependsOn(
        typeof(PermissionManagementDomainModule), 
        typeof(PermissionManagementApplicationContractsModule)
        )]
    public class PermissionManagementApplicationModule : AbpModule
    {
        
    }
}
