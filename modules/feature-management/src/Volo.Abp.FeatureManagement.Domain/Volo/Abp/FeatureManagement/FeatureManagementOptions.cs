using Volo.Abp.Collections;

namespace Volo.Abp.FeatureManagement
{
    public class FeatureManagementOptions
    {
        public ITypeList<IFeatureManagementProvider> Providers { get; }

        public FeatureManagementOptions()
        {
            Providers = new TypeList<IFeatureManagementProvider>();
        }
    }
}