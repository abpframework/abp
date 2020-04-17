using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Settings
{
    public static class SettingProviderExtensions
    {
        public static async Task<bool> IsTrueAsync([NotNull] this ISettingProvider settingProvider, [NotNull] string name)
        {
            Check.NotNull(settingProvider, nameof(settingProvider));
            Check.NotNull(name, nameof(name));

            return string.Equals(
                await settingProvider.GetOrNullAsync(name),
                "true",
                StringComparison.OrdinalIgnoreCase
            );
        }

        public static async Task<T> GetAsync<T>([NotNull] this ISettingProvider settingProvider, [NotNull] string name, T defaultValue = default)
            where T : struct
        {
            Check.NotNull(settingProvider, nameof(settingProvider));
            Check.NotNull(name, nameof(name));

            var value = await settingProvider.GetOrNullAsync(name);
            return value?.To<T>() ?? defaultValue;
        }
    }
}