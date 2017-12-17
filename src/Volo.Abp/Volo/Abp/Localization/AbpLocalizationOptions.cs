namespace Volo.Abp.Localization
{
    public class AbpLocalizationOptions
    {
        public LocalizationResourceDictionary Resources { get; }

        public AbpLocalizationOptions()
        {
            Resources = new LocalizationResourceDictionary();
        }
    }
}
