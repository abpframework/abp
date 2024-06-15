using System.Collections.Generic;
using Volo.Abp;

namespace Volo.CmsKit.Web.Icons;

public static class IconDictionaryHelper
{
    public static string GetLocalizedIcon(
        Dictionary<string, LocalizableIconDictionary> dictionary,
        string name,
        string cultureName = null)
    {
        var icon = dictionary.GetOrDefault(name);
        if (icon == null)
        {
            throw new AbpException($"No icon defined for the item with name '{name}'");
        }

        return icon.GetLocalizedIconOrDefault(cultureName);
    }
}