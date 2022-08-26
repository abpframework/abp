using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Localization.External;

public class NullExternalLocalizationStore : IExternalLocalizationStore, ISingletonDependency
{
    private readonly ExternalLocalizationData _data = new();
    
    public Task SaveAsync()
    {
        return Task.CompletedTask;
    }

    public LocalizationResourceBase GetResourceOrNull(string resourceName)
    {
        return null;
    }

    public Task<ExternalLocalizationData> GetAsync()
    {
        return Task.FromResult(_data);
    }

    public Task<string[]> GetResourceNames()
    {
        return Task.FromResult(Array.Empty<string>());
    }
}