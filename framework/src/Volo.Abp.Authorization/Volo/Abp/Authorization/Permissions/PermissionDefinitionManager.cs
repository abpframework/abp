using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Authorization.Permissions;

public class PermissionDefinitionManager : IPermissionDefinitionManager, ISingletonDependency
{
    private readonly IStaticPermissionDefinitionStore _staticStore;

    public PermissionDefinitionManager(IStaticPermissionDefinitionStore staticStore)
    {
        _staticStore = staticStore;
    }

    public virtual async Task<PermissionDefinition> GetAsync(string name)
    {
        var permission = await GetOrNullAsync(name);
        if (permission == null)
        {
            throw new AbpException("Undefined permission: " + name);
        }

        return permission;
    }

    public virtual Task<PermissionDefinition> GetOrNullAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        return _staticStore.GetOrNullAsync(name);
    }

    public virtual Task<IReadOnlyList<PermissionDefinition>> GetPermissionsAsync()
    {
        return _staticStore.GetPermissionsAsync();
    }

    public Task<IReadOnlyList<PermissionGroupDefinition>> GetGroupsAsync()
    {
        return _staticStore.GetGroupsAsync();
    }
}
