using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.PermissionManagement;

[Dependency(ReplaceServices = true)]
public class DynamicPermissionDefinitionStore : IDynamicPermissionDefinitionStore, ITransientDependency
{
    protected IPermissionGroupDefinitionRecordRepository PermissionGroupRepository { get; }
    protected IPermissionDefinitionRecordRepository PermissionRepository { get; }
    protected IPermissionDefinitionSerializer PermissionDefinitionSerializer { get; }

    public DynamicPermissionDefinitionStore(
        IPermissionGroupDefinitionRecordRepository permissionGroupRepository,
        IPermissionDefinitionRecordRepository permissionRepository,
        IPermissionDefinitionSerializer permissionDefinitionSerializer)
    {
        PermissionGroupRepository = permissionGroupRepository;
        PermissionRepository = permissionRepository;
        PermissionDefinitionSerializer = permissionDefinitionSerializer;
    }

    public virtual async Task<PermissionDefinition> GetOrNullAsync(string name)
    {
        var permissionRecord = await PermissionRepository.FindByNameAsync(name);
        if (permissionRecord == null)
        {
            return null;
        }

        return await PermissionDefinitionSerializer.DeserializeAsync(permissionRecord);
    }

    public virtual async Task<IReadOnlyList<PermissionDefinition>> GetPermissionsAsync()
    {
        var permissionRecords = await PermissionRepository.GetListAsync();
        if (permissionRecords.Count == 0)
        {
            return Array.Empty<PermissionDefinition>();
        }

        var permissionDefinitions = new PermissionDefinition[permissionRecords.Count];
        for (var i = 0; i < permissionRecords.Count; i++)
        {
            permissionDefinitions[i] = await PermissionDefinitionSerializer.DeserializeAsync(permissionRecords[i]);
        }

        return permissionDefinitions;
    }

    public virtual async Task<IReadOnlyList<PermissionGroupDefinition>> GetGroupsAsync()
    {
        var permissionGroupRecords = await PermissionGroupRepository.GetListAsync();
        if (permissionGroupRecords.Count == 0)
        {
            return Array.Empty<PermissionGroupDefinition>();
        }

        var permissionGroupDefinitions = new PermissionGroupDefinition[permissionGroupRecords.Count];
        for (var i = 0; i < permissionGroupRecords.Count; i++)
        {
            permissionGroupDefinitions[i] =
                await PermissionDefinitionSerializer.DeserializeAsync(permissionGroupRecords[i]);
        }

        return permissionGroupDefinitions;
    }
}