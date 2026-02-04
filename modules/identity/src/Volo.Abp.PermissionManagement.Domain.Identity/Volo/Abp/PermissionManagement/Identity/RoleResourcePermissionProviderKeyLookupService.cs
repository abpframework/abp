using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions.Resources;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Localization;

namespace Volo.Abp.PermissionManagement.Identity;

public class RoleResourcePermissionProviderKeyLookupService : IResourcePermissionProviderKeyLookupService, ITransientDependency
{
    public string Name => RoleResourcePermissionValueProvider.ProviderName;

    public ILocalizableString DisplayName { get; }

    protected IUserRoleFinder UserRoleFinder { get; }

    public RoleResourcePermissionProviderKeyLookupService(IUserRoleFinder userRoleFinder)
    {
        UserRoleFinder = userRoleFinder;
        DisplayName = LocalizableString.Create<IdentityResource>(nameof(RoleResourcePermissionProviderKeyLookupService));
    }

    public virtual async Task<List<ResourcePermissionProviderKeyInfo>> SearchAsync(string filter = null, int page = 1, CancellationToken cancellationToken = default)
    {
        var roles = await UserRoleFinder.SearchRoleAsync(filter, page);
        return roles.Select(r => new ResourcePermissionProviderKeyInfo(r.RoleName, r.RoleName)).ToList();
    }

    public virtual Task<List<ResourcePermissionProviderKeyInfo>> SearchAsync(string[] keys, CancellationToken cancellationToken = default)
    {
        // Keys are role names
        return Task.FromResult(keys.Select(x => new ResourcePermissionProviderKeyInfo(x, x)).ToList());
    }
}
