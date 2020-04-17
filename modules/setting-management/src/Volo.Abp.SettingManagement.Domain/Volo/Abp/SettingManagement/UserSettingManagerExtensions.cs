using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Settings;
using Volo.Abp.Users;

namespace Volo.Abp.SettingManagement
{
    //TODO: Consider to move to another package?

    public static class UserSettingManagerExtensions
    {
        public static Task<string> GetOrNullForUserAsync(this ISettingManager settingManager, [NotNull] string name, Guid userId, bool fallback = true)
        {
            return settingManager.GetOrNullAsync(name, UserSettingValueProvider.ProviderName, userId.ToString(), fallback);
        }

        public static Task<string> GetOrNullForCurrentUserAsync(this ISettingManager settingManager, [NotNull] string name, bool fallback = true)
        {
            return settingManager.GetOrNullAsync(name, UserSettingValueProvider.ProviderName, null, fallback);
        }

        public static Task<List<SettingValue>> GetAllForUserAsync(this ISettingManager settingManager, Guid userId, bool fallback = true)
        {
            return settingManager.GetAllAsync(UserSettingValueProvider.ProviderName, userId.ToString(), fallback);
        }

        public static Task<List<SettingValue>> GetAllForCurrentUserAsync(this ISettingManager settingManager, bool fallback = true)
        {
            return settingManager.GetAllAsync(UserSettingValueProvider.ProviderName, null, fallback);
        }

        public static Task SetForUserAsync(this ISettingManager settingManager, Guid userId, [NotNull] string name, [CanBeNull] string value, bool forceToSet = false)
        {
            return settingManager.SetAsync(name, value, UserSettingValueProvider.ProviderName, userId.ToString(), forceToSet);
        }

        public static Task SetForCurrentUserAsync(this ISettingManager settingManager, [NotNull] string name, [CanBeNull] string value, bool forceToSet = false)
        {
            return settingManager.SetAsync(name, value, UserSettingValueProvider.ProviderName, null, forceToSet);
        }
    }
}
