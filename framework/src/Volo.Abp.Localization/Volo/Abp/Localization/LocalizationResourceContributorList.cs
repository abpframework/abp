using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.Localization;

public class LocalizationResourceContributorList : List<ILocalizationResourceContributor>
{
    public LocalizedString GetOrNull(string cultureName, string name)
    {
        this.Reverse();
        foreach (var contributor in this)
        {
            var localString = contributor.GetOrNull(cultureName, name);
            if (localString != null)
            {
                return localString;
            }
        }

        return null;
    }

    public void Fill(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        foreach (var contributor in this)
        {
            contributor.Fill(cultureName, dictionary);
        }
    }
}
