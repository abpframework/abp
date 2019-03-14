namespace Volo.Abp.FeatureManagement
{
    public class EditionFeatureManagementProvider : FeatureManagementProvider
    {
        public const string ProviderName = "Edition";

        public override string Name => ProviderName;

        public EditionFeatureManagementProvider(IFeatureManagementStore store) 
            : base(store)
        {
        }
    }
}