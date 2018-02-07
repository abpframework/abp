using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Session;

namespace Volo.Abp.Settings
{
    public static class UserSettingManagerExtensions
    {
        public static Task<string> GetOrNullForUserAsync(this ISettingManager settingManager, [NotNull] string name, Guid userId, bool fallback = true)
        {
            return settingManager.GetOrNullAsync(name, UserSettingValueProvider.DefaultEntityType, userId.ToString(), fallback);
        }

        public static Task<string> GetOrNullForCurrentUserAsync(this ISettingManager settingManager, [NotNull] string name, bool fallback = true)
        {
            return settingManager.GetOrNullAsync(name, UserSettingValueProvider.DefaultEntityType, null, fallback);
        }

        public static Task<List<SettingValue>> GetAllForUserAsync(this ISettingManager settingManager, Guid userId, bool fallback = true)
        {
            return settingManager.GetAllAsync(UserSettingValueProvider.DefaultEntityType, userId.ToString(), fallback);
        }

        public static Task<List<SettingValue>> GetAllForCurrentUserAsync(this ISettingManager settingManager, bool fallback = true)
        {
            return settingManager.GetAllAsync(UserSettingValueProvider.DefaultEntityType, null, fallback);
        }

        public static Task SetForUserAsync(this ISettingManager settingManager, Guid userId, [NotNull] string name, [CanBeNull] string value)
        {
            return settingManager.SetAsync(name, value, UserSettingValueProvider.DefaultEntityType, userId.ToString());
        }

        public static Task SetForCurrentUserAsync(this ISettingManager settingManager, [NotNull] string name, [CanBeNull] string value)
        {
            return settingManager.SetAsync(name, value, UserSettingValueProvider.DefaultEntityType, null);
        }
    }
}
