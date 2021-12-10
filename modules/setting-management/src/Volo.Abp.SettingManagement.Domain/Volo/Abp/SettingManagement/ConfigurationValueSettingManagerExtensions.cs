using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Settings;

namespace Volo.Abp.SettingManagement;

public static class ConfigurationValueSettingManagerExtensions
{
    public static Task<string> GetOrNullConfigurationAsync(this ISettingManager settingManager, [NotNull] string name, bool fallback = true)
    {
        return settingManager.GetOrNullAsync(name, ConfigurationSettingValueProvider.ProviderName, null, fallback);
    }

    public static Task<List<SettingValue>> GetAllConfigurationAsync(this ISettingManager settingManager, bool fallback = true)
    {
        return settingManager.GetAllAsync(ConfigurationSettingValueProvider.ProviderName, null, fallback);
    }
}
