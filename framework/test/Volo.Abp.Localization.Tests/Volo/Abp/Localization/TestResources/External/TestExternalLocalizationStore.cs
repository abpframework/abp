using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization.External;

namespace Volo.Abp.Localization.TestResources.External;

public class TestExternalLocalizationStore : IExternalLocalizationStore, ITransientDependency
{
    private readonly IDictionary<string, LocalizationResourceBase> _resources = new LocalizationResourceDictionary();

    public LocalizationResourceBase GetResourceOrNull(string resourceName)
    {
        return GetOrCreateResource(resourceName);
    }

    public Task<LocalizationResourceBase> GetResourceOrNullAsync(string resourceName)
    {
        return Task.FromResult(GetOrCreateResource(resourceName));
    }

    public Task<string[]> GetResourceNamesAsync()
    {
        return Task.FromResult(new []{TestExternalResourceNames.ExternalResource1, TestExternalResourceNames.ExternalResource2});
    }

    public async Task<LocalizationResourceBase[]> GetResourcesAsync()
    {
        var resourceNames = await GetResourceNamesAsync();
        return resourceNames.Select(GetResourceOrNull).ToArray();
    }
    
    private LocalizationResourceBase GetOrCreateResource(string resourceName)
    {
        if(resourceName != TestExternalResourceNames.ExternalResource1 && resourceName != TestExternalResourceNames.ExternalResource2)
        {
            return null;
        }

        return _resources.GetOrAdd(resourceName, name => new NonTypedLocalizationResource(name));
    }
    
    public class TestExternalResourceNames
    {
        public const string ExternalResource1 = "TestExternalResource1";
        
        public const string ExternalResource2 = "TestExternalResource2";
    }
}