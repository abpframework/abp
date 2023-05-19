using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Authorization.Permissions;

public class NullDynamicPermissionDefinitionStore : IDynamicPermissionDefinitionStore, ISingletonDependency
{
    private readonly static Task<PermissionDefinition> CachedPermissionResult = Task.FromResult((PermissionDefinition)null);
    
    private readonly static Task<IReadOnlyList<PermissionDefinition>> CachedPermissionsResult =
        Task.FromResult((IReadOnlyList<PermissionDefinition>)Array.Empty<PermissionDefinition>().ToImmutableList());

    private readonly static Task<IReadOnlyList<PermissionGroupDefinition>> CachedGroupsResult =
        Task.FromResult((IReadOnlyList<PermissionGroupDefinition>)Array.Empty<PermissionGroupDefinition>().ToImmutableList());

    public Task<PermissionDefinition> GetOrNullAsync(string name)
    {
        return CachedPermissionResult;
    }

    public Task<IReadOnlyList<PermissionDefinition>> GetPermissionsAsync()
    {
        return CachedPermissionsResult;
    }

    public Task<IReadOnlyList<PermissionGroupDefinition>> GetGroupsAsync()
    {
        return CachedGroupsResult;
    }
}