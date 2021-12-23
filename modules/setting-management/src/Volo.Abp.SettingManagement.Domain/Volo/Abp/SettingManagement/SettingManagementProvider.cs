using System.Threading.Tasks;
using Volo.Abp.Settings;

namespace Volo.Abp.SettingManagement;

public abstract class SettingManagementProvider : ISettingManagementProvider
{
    public abstract string Name { get; }

    //TODO: Rename to Store
    protected ISettingManagementStore SettingManagementStore { get; }

    protected SettingManagementProvider(ISettingManagementStore settingManagementStore)
    {
        SettingManagementStore = settingManagementStore;
    }

    public virtual async Task<string> GetOrNullAsync(SettingDefinition setting, string providerKey)
    {
        return await SettingManagementStore.GetOrNullAsync(setting.Name, Name, NormalizeProviderKey(providerKey));
    }

    public virtual async Task SetAsync(SettingDefinition setting, string value, string providerKey)
    {
        await SettingManagementStore.SetAsync(setting.Name, value, Name, NormalizeProviderKey(providerKey));
    }

    public virtual async Task ClearAsync(SettingDefinition setting, string providerKey)
    {
        await SettingManagementStore.DeleteAsync(setting.Name, Name, NormalizeProviderKey(providerKey));
    }

    protected virtual string NormalizeProviderKey(string providerKey)
    {
        return providerKey;
    }
}
