using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Volo.Abp.AspNetCore.Mvc.Client;

public class RemoteLanguageProvider : ILanguageProvider, ITransientDependency
{
    protected ICachedApplicationConfigurationClient ConfigurationClient { get; }

    public RemoteLanguageProvider(ICachedApplicationConfigurationClient configurationClient)
    {
        ConfigurationClient = configurationClient;
    }

    public async Task<IReadOnlyList<LanguageInfo>> GetLanguagesAsync()
    {
        var configuration = await ConfigurationClient.GetAsync();
        return configuration.Localization.Languages;
    }
}
