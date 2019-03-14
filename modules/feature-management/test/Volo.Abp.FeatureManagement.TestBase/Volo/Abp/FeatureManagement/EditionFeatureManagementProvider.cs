using Volo.Abp.Features;

namespace Volo.Abp.FeatureManagement
{
    public class EditionFeatureManagementProvider : FeatureManagementProvider
    {
        public override string Name => EditionFeatureValueProvider.ProviderName;

        public EditionFeatureManagementProvider(IFeatureManagementStore store) 
            : base(store)
        {
        }
    }
}