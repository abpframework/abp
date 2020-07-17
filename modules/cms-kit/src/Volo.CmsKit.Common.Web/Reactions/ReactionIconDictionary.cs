using System.Collections.Generic;
using Volo.Abp;
using Volo.CmsKit.Web.Icons;

namespace Volo.CmsKit.Web.Reactions
{
    public class ReactionIconDictionary : Dictionary<string, LocalizableIconDictionary>
    {
        public string GetLocalizedIcon(string name, string cultureName = null)
        {
            var icon = this.GetOrDefault(name);
            if (icon == null)
            {
                throw new AbpException($"No icon defined for the reaction with name '{name}'");
            }

            return icon.GetLocalizedIconOrDefault(cultureName);
        }
    }
}
