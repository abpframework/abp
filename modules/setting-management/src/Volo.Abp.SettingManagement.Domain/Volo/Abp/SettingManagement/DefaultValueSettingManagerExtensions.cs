using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Settings;

namespace Volo.Abp.SettingManagement
{
    public static class DefaultValueSettingManagerExtensions
    {
        public static Task<string> GetOrNullDefaultAsync(this ISettingManager settingManager, [NotNull] string name, bool fallback = true)
        {
            return settingManager.GetOrNullAsync(name, DefaultValueSettingValueProvider.ProviderName, null, fallback);
        }

        public static Task<List<SettingValue>> GetAllDefaultAsync(this ISettingManager settingManager, bool fallback = true)
        {
            return settingManager.GetAllAsync(DefaultValueSettingValueProvider.ProviderName, null, fallback);
        }
    }
}