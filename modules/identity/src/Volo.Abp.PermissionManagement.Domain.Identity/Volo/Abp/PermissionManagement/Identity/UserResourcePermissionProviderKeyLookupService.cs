using System;
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

public class UserResourcePermissionProviderKeyLookupService : IResourcePermissionProviderKeyLookupService, ITransientDependency
{
    public string Name => UserResourcePermissionValueProvider.ProviderName;

    public ILocalizableString DisplayName { get; }

    protected IUserRoleFinder UserRoleFinder { get; }

    public UserResourcePermissionProviderKeyLookupService(IUserRoleFinder userRoleFinder)
    {
        UserRoleFinder = userRoleFinder;
        DisplayName = LocalizableString.Create<IdentityResource>(nameof(UserResourcePermissionProviderKeyLookupService));
    }

    public virtual Task<bool> IsAvailableAsync()
    {
        return Task.FromResult(true);
    }

    public virtual async Task<List<ResourcePermissionProviderKeyInfo>> SearchAsync(string filter = null, int page = 1, CancellationToken cancellationToken = default)
    {
        var users = await UserRoleFinder.SearchUserAsync(filter, page);
        return users.Select(u => new ResourcePermissionProviderKeyInfo(u.Id.ToString(), u.UserName)).ToList();
    }

    public virtual async Task<List<ResourcePermissionProviderKeyInfo>> SearchAsync(string[] keys, CancellationToken cancellationToken = default)
    {
        var ids = keys
            .Select(key => Guid.TryParse(key, out var id) ? (Guid?)id : null)
            .Where(id => id.HasValue)
            .Select(id => id.Value)
            .Distinct()
            .ToArray();
        var users = await UserRoleFinder.SearchUserByIdsAsync(ids.ToArray());
        return users.Select(u => new ResourcePermissionProviderKeyInfo(u.Id.ToString(), u.UserName)).ToList();
    }
}
