using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Session;

namespace Volo.Abp.Permissions
{
    public static class UserPermissionManagerExtensions
    {
        public static Task<bool> IsGrantedForUserAsync(this IPermissionManager permissionManager, [NotNull] string name, Guid userId, bool fallback = true)
        {
            return permissionManager.IsGrantedAsync(name, UserPermissionValueProvider.ProviderName, userId.ToString(), fallback);
        }

        public static Task<bool> IsGrantedForCurrentUserAsync(this IPermissionManager permissionManager, [NotNull] string name, bool fallback = true)
        {
            return permissionManager.IsGrantedAsync(name, UserPermissionValueProvider.ProviderName, null, fallback);
        }

        public static Task<List<PermissionGrantInfo>> GetAllForUserAsync(this IPermissionManager permissionManager, Guid userId, bool fallback = true)
        {
            return permissionManager.GetAllAsync(UserPermissionValueProvider.ProviderName, userId.ToString(), fallback);
        }

        public static Task<List<PermissionGrantInfo>> GetAllForCurrentUserAsync(this IPermissionManager permissionManager, bool fallback = true)
        {
            return permissionManager.GetAllAsync(UserPermissionValueProvider.ProviderName, null, fallback);
        }

        public static Task SetForUserAsync(this IPermissionManager permissionManager, Guid userId, [NotNull] string name, bool isGranted, bool forceToSet = false)
        {
            return permissionManager.SetAsync(name, isGranted, UserPermissionValueProvider.ProviderName, userId.ToString(), forceToSet);
        }

        public static Task SetForCurrentUserAsync(this IPermissionManager permissionManager, [NotNull] string name, bool isGranted, bool forceToSet = false)
        {
            return permissionManager.SetAsync(name, isGranted, UserPermissionValueProvider.ProviderName, null, forceToSet);
        }
    }
}
