using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.Localization;

public class LocalizationResourceContributorList : List<ILocalizationResourceContributor>
{
    public LocalizedString GetOrNull(
        string cultureName,
        string name,
        bool includeDynamicContributors = true)
    {
        foreach (var contributor in this.Select(x => x).Reverse())
        {
            if (!includeDynamicContributors && contributor.IsDynamic)
            {
                continue;
            }
            
            var localString = contributor.GetOrNull(cultureName, name);
            if (localString != null)
            {
                return localString;
            }
        }

        return null;
    }

    public void Fill(
        string cultureName, 
        Dictionary<string, LocalizedString> dictionary,
        bool includeDynamicContributors = true)
    {
        foreach (var contributor in this)
        {
            if (!includeDynamicContributors && contributor.IsDynamic)
            {
                continue;
            }
            
            contributor.Fill(cultureName, dictionary);
        }
    }
    
    public async Task FillAsync(
        string cultureName, 
        Dictionary<string, LocalizedString> dictionary,
        bool includeDynamicContributors = true)
    {
        foreach (var contributor in this)
        {
            if (!includeDynamicContributors && contributor.IsDynamic)
            {
                continue;
            }
            
            await contributor.FillAsync(cultureName, dictionary);
        }
    }

    internal async Task<IEnumerable<string>> GetSupportedCulturesAsync()
    {
        var cultures = new List<string>();

        foreach (var contributor in this)
        {
            cultures.AddRange(await contributor.GetSupportedCulturesAsync());
        }

        return cultures;
    }
}
