using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.PermissionManagement;

public interface IDynamicPermissionDefinitionStoreInMemoryCache
{
    string CacheStamp { get; set; }
    
    SemaphoreSlim SyncSemaphore { get; }
    
    Task FillAsync(
        List<PermissionGroupDefinitionRecord> permissionGroupRecords,
        List<PermissionDefinitionRecord> permissionRecords);

    PermissionDefinition GetPermissionOrNull(string name);
    
    IReadOnlyList<PermissionDefinition> GetPermissions();
    
    IReadOnlyList<PermissionGroupDefinition> GetGroups();
}