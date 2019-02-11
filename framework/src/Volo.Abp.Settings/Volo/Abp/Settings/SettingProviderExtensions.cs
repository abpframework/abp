using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Threading;

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

        public static string GetOrNull([NotNull] this ISettingProvider settingProvider, [NotNull] string name)
        {
            Check.NotNull(settingProvider, nameof(settingProvider));
            return AsyncHelper.RunSync(() => settingProvider.GetOrNullAsync(name));
        }

        public static List<SettingValue> GetAll([NotNull] this ISettingProvider settingProvider)
        {
            Check.NotNull(settingProvider, nameof(settingProvider));
            return AsyncHelper.RunSync(settingProvider.GetAllAsync);
        }

        public static T Get<T>([NotNull] this ISettingProvider settingProvider, [NotNull] string name, T defaultValue = default)
            where T : struct
        {
            return AsyncHelper.RunSync(() => settingProvider.GetAsync(name, defaultValue));
        }

        public static bool IsTrue([NotNull] this ISettingProvider settingProvider, [NotNull] string name)
        {
            return AsyncHelper.RunSync(() => settingProvider.IsTrueAsync(name));
        }
    }
}