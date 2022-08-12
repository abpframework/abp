using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.PermissionManagement;

public interface IPermissionDefinitionSerializer
{
    public Task<PermissionDefinitionRecord> SerializeAsync(
        PermissionDefinition permission,
        [CanBeNull] PermissionGroupDefinition permissionGroup);
    
    public Task<PermissionDefinition> DeserializeAsync(
        PermissionDefinitionRecord permissionRecord);
    
    public Task<PermissionGroupDefinitionRecord> SerializeAsync(
        PermissionGroupDefinition permissionGroup);

    public Task<PermissionGroupDefinition> DeserializeAsync(
        PermissionGroupDefinitionRecord permissionGroupRecord);
}