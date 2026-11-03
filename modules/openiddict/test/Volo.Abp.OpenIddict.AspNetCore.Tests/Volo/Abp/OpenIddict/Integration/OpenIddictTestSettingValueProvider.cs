using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Settings;

namespace Volo.Abp.OpenIddict.Integration;

public class OpenIddictTestSettingValueProvider : ISettingValueProvider
{
    public const string ProviderName = "OpenIddictIntegrationTest";

    private readonly Dictionary<string, string> _values = new();

    public string Name => ProviderName;

    public void Set(string name, string value)
    {
        _values[name] = value;
    }

    public void Clear()
    {
        _values.Clear();
    }

    public Task<string> GetOrNullAsync(SettingDefinition setting)
    {
        return Task.FromResult(_values.GetOrDefault(setting.Name));
    }

    public Task<List<SettingValue>> GetAllAsync(SettingDefinition[] settings)
    {
        return Task.FromResult(settings
            .Select(setting => new SettingValue(setting.Name, _values.GetOrDefault(setting.Name)))
            .ToList());
    }
}
