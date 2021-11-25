using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace Volo.Abp.SettingManagement;

public class DefaultValueSettingManagementProvider : ISettingManagementProvider, ISingletonDependency
{
    public string Name => DefaultValueSettingValueProvider.ProviderName;

    public virtual Task<string> GetOrNullAsync(SettingDefinition setting, string providerKey)
    {
        return Task.FromResult(setting.DefaultValue);
    }

    public virtual Task SetAsync(SettingDefinition setting, string value, string providerKey)
    {
        throw new AbpException($"Can not set default value of a setting. It is only possible while defining the setting in a {typeof(ISettingDefinitionProvider)} implementation.");
    }

    public virtual Task ClearAsync(SettingDefinition setting, string providerKey)
    {
        throw new AbpException($"Can not clear default value of a setting. It is only possible while defining the setting in a {typeof(ISettingDefinitionProvider)} implementation.");
    }
}
