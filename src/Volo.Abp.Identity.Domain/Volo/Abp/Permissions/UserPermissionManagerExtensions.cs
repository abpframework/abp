using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Identity;

namespace Volo.Abp.Permissions
{
    public static class UserPermissionManagerExtensions
    {
        public static Task<List<PermissionWithGrantedProviders>> GetAllForUserAsync([NotNull] this IPermissionManager permissionManager, Guid userId)
        {
            Check.NotNull(permissionManager, nameof(permissionManager));

            return permissionManager.GetAllAsync(UserPermissionManagementProvider.ProviderName, userId.ToString());
        }

        public static Task SetForUserAsync([NotNull] this IPermissionManager permissionManager, Guid userId, [NotNull] string name, bool isGranted)
        {
            Check.NotNull(permissionManager, nameof(permissionManager));

            return permissionManager.SetAsync(name, UserPermissionManagementProvider.ProviderName, userId.ToString(), isGranted);
        }
    }
}
