using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Volo.Abp.PermissionManagement
{
    [DependsOn(typeof(DddApplicationModule))]
    [DependsOn(typeof(PermissionManagementDomainSharedModule))]
    public class PermissionManagementApplicationContractsModule : AbpModule
    {
        
    }
}
