using System.Collections.Generic;
using Volo.Abp;

namespace Volo.CmsKit.Web.Icons;

public class LocalizableIconDictionaryBase : Dictionary<string, LocalizableIconDictionary>
{
    public string GetLocalizedIcon(string name, string cultureName = null)
    {
        var icon = this.GetOrDefault(name);
        if (icon == null)
        {
            throw new AbpException($"No icon defined for the item with name '{name}'");
        }

        return icon.GetLocalizedIconOrDefault(cultureName);
    }
}