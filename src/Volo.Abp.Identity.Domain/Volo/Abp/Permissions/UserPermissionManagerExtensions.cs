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
            throw new NotImplementedException();
            //return permissionManager.GetAsync(name, ProviderName, userId.ToString());
        }

        public static Task<List<PermissionWithGrantedProviders>> GetAllAsync(this IPermissionManager permissionManager, Guid userId)
        {
            return permissionManager.GetAllAsync(ProviderName, userId.ToString());
        }
        
        public static Task GrantForUserAsync(this IPermissionManager permissionManager, Guid userId, [NotNull] string name)
        {
            throw new NotImplementedException();
            //return permissionManager.GrantAsync(name, ProviderName, userId.ToString());
        }

        public static Task RevokeForUserAsync(this IPermissionManager permissionManager, Guid userId, [NotNull] string name)
        {
            throw new NotImplementedException();
            //return permissionManager.RevokeAsync(name, ProviderName, userId.ToString());
        }
    }
}
