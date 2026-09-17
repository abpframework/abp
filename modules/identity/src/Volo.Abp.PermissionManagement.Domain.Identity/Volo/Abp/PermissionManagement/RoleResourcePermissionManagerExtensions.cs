using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.PermissionManagement;

public static class RoleResourceresourcePermissionManagerExtensions
{
    public static Task<PermissionWithGrantedProviders> GetForRoleAsync([NotNull] this IResourcePermissionManager resourcePermissionManager, string roleName, string permissionName, [NotNull] string resourceName, [NotNull] string resourceKey)
    {
        Check.NotNull(resourcePermissionManager, nameof(resourcePermissionManager));

        return resourcePermissionManager.GetAsync(permissionName, resourceName, resourceKey, RolePermissionValueProvider.ProviderName, roleName);
    }

    public static Task<List<PermissionWithGrantedProviders>> GetAllForRoleAsync([NotNull] this IResourcePermissionManager resourcePermissionManager, string roleName, [NotNull] string resourceName, [NotNull] string resourceKey)
    {
        Check.NotNull(resourcePermissionManager, nameof(resourcePermissionManager));

        return resourcePermissionManager.GetAllAsync(resourceName, resourceKey, RolePermissionValueProvider.ProviderName, roleName);
    }

    public static Task SetForRoleAsync([NotNull] this IResourcePermissionManager resourcePermissionManager, string roleName, [NotNull] string permissionName, [NotNull] string resourceName, [NotNull] string resourceKey, bool isGranted)
    {
        Check.NotNull(resourcePermissionManager, nameof(resourcePermissionManager));

        return resourcePermissionManager.SetAsync(permissionName, resourceName, resourceKey, RolePermissionValueProvider.ProviderName, roleName, isGranted);
    }
}
