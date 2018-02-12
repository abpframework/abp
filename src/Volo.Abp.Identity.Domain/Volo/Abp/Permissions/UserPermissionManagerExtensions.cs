using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Permissions
{
    public static class UserPermissionManagerExtensions
    {
        private const string ProviderName = "User"; //TODO: Share same const with UserPermissionValueProvider

        public static Task<bool> IsGrantedForUserAsync(this IPermissionManager permissionManager, [NotNull] string name, Guid userId)
        {
            return permissionManager.IsGrantedAsync(name, ProviderName, userId.ToString());
        }

        public static Task<List<string>> GetAllGrantedForUserAsync(this IPermissionManager permissionManager, Guid userId)
        {
            return permissionManager.GetAllGrantedAsync(ProviderName, userId.ToString());
        }
        
        public static Task GrantForUserAsync(this IPermissionManager permissionManager, Guid userId, [NotNull] string name)
        {
            return permissionManager.GrantAsync(name, ProviderName, userId.ToString());
        }

        public static Task RevokeForUserAsync(this IPermissionManager permissionManager, Guid userId, [NotNull] string name)
        {
            return permissionManager.RevokeAsync(name, ProviderName, userId.ToString());
        }
    }
}
