namespace Volo.Abp.Localization
{
    public class AbpLocalizationOptions
    {
        public LocalizationResourceList Resources { get; }
        public AbpStringLocalizerList Resolvers { get; }

        public AbpLocalizationOptions()
        {
            Resources = new LocalizationResourceList();
        }
    }
}
