using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.Permissions;

namespace Volo.Abp.Session
{
    public class UserPermissionValueProvider : PermissionValueProvider
    {
        public const string ProviderName = "User";

        public override string Name => ProviderName;

        protected ICurrentUser CurrentUser { get; }

        public UserPermissionValueProvider(IPermissionStore permissionStore, ICurrentUser currentUser)
            : base(permissionStore)
        {
            CurrentUser = currentUser;
        }

        public override async Task<bool?> IsGrantedAsync(PermissionDefinition permission, string providerName, string providerKey)
        {
            var userId = ParseOrGetCurrentUser(providerName, providerKey);
            if (userId == null)
            {
                return null;
            }

            return await PermissionStore.IsGrantedAsync(permission.Name, Name, userId.ToString());
        }

        protected virtual Guid? ParseOrGetCurrentUser(string providerName, string providerKey)
        {
            if (providerName == null)
            {
                return CurrentUser.Id;
            }

            if (providerName == Name)
            {
                if (providerKey == null)
                {
                    return CurrentUser.Id;
                }

                if (!Guid.TryParse(providerKey, out var result))
                {
                    throw new AbpException("UserId should be a Guid!");
                }

                return result;
            }

            return null;
        }

        public override Task SetAsync(PermissionDefinition permission, bool isGranted, string providerKey)
        {
            var userId = ParseOrGetCurrentUser(Name, providerKey);
            if (userId == null)
            {
                Logger.LogWarning($"Could not set the permission '{permission}' because the user id is not available!");
                return Task.CompletedTask;
            }

            //TODO: Seperate SetAsync to AddGrant / RemoveGrant

            if (isGranted)
            {
                return PermissionStore.AddAsync(permission.Name, Name, userId.ToString());
            }
            else
            {
                return PermissionStore.RemoveAsync(permission.Name, Name, userId.ToString());
            }
        }

        public override Task ClearAsync(PermissionDefinition permission, string providerKey)
        {
            var userId = ParseOrGetCurrentUser(Name, providerKey);
            if (userId == null)
            {
                Logger.LogWarning($"Could not clear the permission '{permission}' because the user id is not available!");
                return Task.CompletedTask;
            }

            return PermissionStore.RemoveAsync(permission.Name, Name, providerKey);
        }
    }
}
