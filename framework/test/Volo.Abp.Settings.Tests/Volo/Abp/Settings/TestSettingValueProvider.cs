using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Settings;

public class TestSettingValueProvider : ISettingValueProvider, ITransientDependency
{
    public const string ProviderName = "Test";

    private readonly Dictionary<string, string> _values;

    public string Name => ProviderName;

    public TestSettingValueProvider()
    {
        _values = new Dictionary<string, string>();
    }

    public Task<string> GetOrNullAsync(SettingDefinition setting)
    {
        return Task.FromResult(_values.GetOrDefault(setting.Name));
    }

    public Task<List<SettingValue>> GetAllAsync(SettingDefinition[] settings)
    {
        return Task.FromResult(settings.Select(x => new SettingValue(x.Name, _values.GetOrDefault(x.Name))).ToList());
    }
}
