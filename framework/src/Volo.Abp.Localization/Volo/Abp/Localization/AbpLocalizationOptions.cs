using System.Collections.Generic;
using Volo.Abp.Collections;

namespace Volo.Abp.Localization
{
    public class AbpLocalizationOptions
    {
        public LocalizationResourceDictionary Resources { get; }

        public ITypeList<ILocalizationResourceContributor> GlobalContributors { get; }

        public List<LanguageInfo> Languages { get; }

        public AbpLocalizationOptions()
        {
            Resources = new LocalizationResourceDictionary();
            GlobalContributors = new TypeList<ILocalizationResourceContributor>();
            Languages = new List<LanguageInfo>();
        }
    }
}
