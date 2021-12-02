using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Settings;

namespace Volo.Abp.AspNetCore.Mvc.Client;

public class RemoteSettingProvider : ISettingProvider, ITransientDependency
{
    protected ICachedApplicationConfigurationClient ConfigurationClient { get; }

    public RemoteSettingProvider(ICachedApplicationConfigurationClient configurationClient)
    {
        ConfigurationClient = configurationClient;
    }

    public async Task<string> GetOrNullAsync(string name)
    {
        var configuration = await ConfigurationClient.GetAsync();
        return configuration.Setting.Values.GetOrDefault(name);
    }

    public async Task<List<SettingValue>> GetAllAsync(string[] names)
    {
        var configuration = await ConfigurationClient.GetAsync();
        return names.Select(x => new SettingValue(x, configuration.Setting.Values.GetOrDefault(x))).ToList();
    }

    public async Task<List<SettingValue>> GetAllAsync()
    {
        var configuration = await ConfigurationClient.GetAsync();
        return configuration
            .Setting.Values
            .Select(s => new SettingValue(s.Key, s.Value))
            .ToList();
    }
}
