using System.Collections.Generic;
using System.Linq;
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

    internal IEnumerable<string> GetSupportedCultures()
    {
        return this.SelectMany(c => c.GetSupportedCultures()).Distinct();
    }
}
