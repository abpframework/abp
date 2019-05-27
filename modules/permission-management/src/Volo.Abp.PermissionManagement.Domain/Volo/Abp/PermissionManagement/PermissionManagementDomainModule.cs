using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Volo.Abp.Domain;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace Volo.Abp.PermissionManagement
{
    [DependsOn(typeof(AuthorizationModule))]
    [DependsOn(typeof(DddDomainModule))]
    [DependsOn(typeof(PermissionManagementDomainSharedModule))]
    [DependsOn(typeof(CachingModule))]
    [DependsOn(typeof(JsonModule))]
    public class PermissionManagementDomainModule : AbpModule
    {
        
    }
}
