namespace Volo.Abp.Localization
{
    public class AbpLocalizationOptions
    {
        public LocalizationResourceList Resources { get; }

        public AbpLocalizationOptions()
        {
            Resources = new LocalizationResourceList();
        }
    }
}
