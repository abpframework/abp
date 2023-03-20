using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.Localization.TestResources.External;

public class TestExternalLocalizationContributor : ILocalizationResourceContributor
{
    public bool IsDynamic => true;

    private string ResourceName { get; set; }

    public void Initialize(LocalizationResourceInitializationContext context)
    {
        ResourceName = context.Resource.ResourceName;
    }

    public LocalizedString GetOrNull(string cultureName, string name)
    {
        switch (ResourceName)
        {
            case TestExternalLocalizationStore.TestExternalResourceNames.ExternalResource1:
            case TestExternalLocalizationStore.TestExternalResourceNames.ExternalResource2:
                return new LocalizedString(name, name );
            default:
                return null;
        }
    }

    public void Fill(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
    }

    public Task FillAsync(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        return Task.CompletedTask;
    }

    public Task<IEnumerable<string>> GetSupportedCulturesAsync()
    {
        return Task.FromResult(new List<string>().AsEnumerable());
    }
}