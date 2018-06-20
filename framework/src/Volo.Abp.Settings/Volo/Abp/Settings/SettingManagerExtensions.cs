using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Settings
{
    public static class SettingManagerExtensions
    {
        public static async Task<bool> IsTrueAsync([NotNull] this ISettingManager settingManager, [NotNull] string name)
        {
            Check.NotNull(settingManager, nameof(settingManager));
            Check.NotNull(name, nameof(name));

            return string.Equals(
                await settingManager.GetOrNullAsync(name),
                "true",
                StringComparison.OrdinalIgnoreCase
            );
        }

        public static async Task<T> GetAsync<T>([NotNull] this ISettingManager settingManager, [NotNull] string name, T defaultValue = default) 
            where T : struct
        {
            Check.NotNull(settingManager, nameof(settingManager));
            Check.NotNull(name, nameof(name));

            var value = await settingManager.GetOrNullAsync(name);
            return value?.To<T>() ?? defaultValue;
        }
    }
}