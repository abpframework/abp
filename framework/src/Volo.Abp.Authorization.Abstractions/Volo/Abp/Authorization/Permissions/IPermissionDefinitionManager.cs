using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Authorization.Permissions;

public interface IPermissionDefinitionManager
{
    [ItemNotNull]
    Task<PermissionDefinition> GetAsync([NotNull] string name);

    Task<PermissionDefinition?> GetOrNullAsync([NotNull] string name);

    [ItemNotNull]
    Task<PermissionDefinition> GetResourcePermissionAsync([NotNull]string resourceName, [NotNull] string name);

    Task<PermissionDefinition?> GetResourcePermissionOrNullAsync([NotNull]string resourceName, [NotNull] string name);

    Task<IReadOnlyList<PermissionDefinition>> GetPermissionsAsync();

    Task<IReadOnlyList<PermissionDefinition>> GetResourcePermissionsAsync();

    Task<IReadOnlyList<PermissionGroupDefinition>> GetGroupsAsync();
}
