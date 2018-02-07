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
            return settingManager.GetOrNullAsync(name, UserSettingContributor.DefaultEntityType, userId.ToString(), fallback);
        }

        public static Task<List<SettingValue>> GetAllForUserAsync(this ISettingManager settingManager, Guid userId, bool fallback = true)
        {
            return settingManager.GetAllAsync(UserSettingContributor.DefaultEntityType, userId.ToString(), fallback);
        }

        public static Task SetForUserAsync(this ISettingManager settingManager, Guid userId, [NotNull] string name, [CanBeNull] string value)
        {
            return settingManager.SetAsync(name, value, UserSettingContributor.DefaultEntityType, userId.ToString());
        }
    }
}
