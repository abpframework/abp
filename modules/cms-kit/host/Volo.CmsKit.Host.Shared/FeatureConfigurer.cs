using Volo.Abp.GlobalFeatures;

namespace Volo.CmsKit
{
    public static class FeatureConfigurer
    {
        public static void Configure()
        {
            GlobalFeatureManager.Instance.Modules().CmsKit().EnableAll();
        }
    }
}
