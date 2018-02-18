using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Identity;

namespace Volo.Abp.Permissions
{
    public static class RolePermissionManagerExtensions
    {
        public static Task<PermissionWithGrantedProviders> GetForRoleAsync([NotNull] this IPermissionManager permissionManager, string roleName, string permissionName)
        {
            Check.NotNull(permissionManager, nameof(permissionManager));

            return permissionManager.GetAsync(permissionName, RolePermissionManagementProvider.ProviderName, roleName);
        }

        public static Task<List<PermissionWithGrantedProviders>> GetAllForRoleAsync([NotNull] this IPermissionManager permissionManager, string roleName)
        {
            Check.NotNull(permissionManager, nameof(permissionManager));

            return permissionManager.GetAllAsync(RolePermissionManagementProvider.ProviderName, roleName);
        }

        public static Task SetForRoleAsync([NotNull] this IPermissionManager permissionManager, string roleName, [NotNull] string permissionName, bool isGranted)
        {
            Check.NotNull(permissionManager, nameof(permissionManager));

            return permissionManager.SetAsync(permissionName, RolePermissionManagementProvider.ProviderName, roleName, isGranted);
        }
    }
}