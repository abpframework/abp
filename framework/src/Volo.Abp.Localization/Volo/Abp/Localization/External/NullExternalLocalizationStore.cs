using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Localization.External;

public class NullExternalLocalizationStore : IExternalLocalizationStore, ISingletonDependency
{
    public LocalizationResourceBase GetResourceOrNull(string resourceName)
    {
        return null;
    }

    public Task<string[]> GetResourceNamesAsync()
    {
        return Task.FromResult(Array.Empty<string>());
    }
}