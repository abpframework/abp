using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.PermissionManagement;

public interface IPermissionDefinitionSerializer
{
    Task<(PermissionGroupDefinitionRecord[], PermissionDefinitionRecord[])>
        SerializeAsync(IEnumerable<PermissionGroupDefinition> permissionGroups);

    Task<PermissionGroupDefinitionRecord> SerializeAsync(
        PermissionGroupDefinition permissionGroup);

    Task<PermissionDefinitionRecord> SerializeAsync(
        PermissionDefinition permission,
        [CanBeNull] PermissionGroupDefinition permissionGroup);
}