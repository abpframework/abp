using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace Volo.Abp.Identity;

public class TestSettingValueProvider : ISettingValueProvider, ITransientDependency
{
    public const string ProviderName = "Test";

    private readonly static Dictionary<string, string> Values = new ();

    public string Name => ProviderName;

    public static void AddSetting(string name, string value)
    {
        Values[name] = value;
    }

    public Task<string> GetOrNullAsync(SettingDefinition setting)
    {
        return Task.FromResult(Values.GetOrDefault(setting.Name));
    }

    public Task<List<SettingValue>> GetAllAsync(SettingDefinition[] settings)
    {
        return Task.FromResult(settings.Select(x => new SettingValue(x.Name, Values.GetOrDefault(x.Name))).ToList());
    }
}
