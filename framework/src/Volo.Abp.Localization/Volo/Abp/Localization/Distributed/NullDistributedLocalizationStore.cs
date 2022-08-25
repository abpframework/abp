using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Localization.Distributed;

public class NullDistributedLocalizationStore : IDistributedLocalizationStore, ISingletonDependency
{
    private readonly DistributedLocalizationData _data = new();
    
    public Task SaveAsync()
    {
        return Task.CompletedTask;
    }

    public Task<DistributedLocalizationData> GetAsync()
    {
        return Task.FromResult(_data);
    }

    public Task<string[]> GetResourceNames()
    {
        return Task.FromResult(Array.Empty<string>());
    }
}