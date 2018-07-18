using Volo.Abp.Collections;

namespace Volo.Abp.Localization
{
    public class AbpLocalizationOptions
    {
        public LocalizationResourceDictionary Resources { get; }

        public ITypeList<ILocalizationResourceContributor> GlobalContributors { get; }

        public AbpLocalizationOptions()
        {
            Resources = new LocalizationResourceDictionary();
            GlobalContributors = new TypeList<ILocalizationResourceContributor>();
        }
    }
}
