using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.Abp.Authorization.Permissions;

public interface IDynamicPermissionDefinitionStore
{
    Task<PermissionDefinition?> GetOrNullAsync(string name);

    Task<IReadOnlyList<PermissionDefinition>> GetPermissionsAsync();

    Task<PermissionDefinition?> GetResourcePermissionOrNullAsync(string resourceName, string name);

    Task<IReadOnlyList<PermissionDefinition>> GetResourcePermissionsAsync();

    Task<IReadOnlyList<PermissionGroupDefinition>> GetGroupsAsync();
}
