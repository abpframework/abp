using Volo.Abp.GlobalFeatures;
using Volo.Abp.Threading;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit
{
    public static class FeatureConfigurer
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                GlobalFeatureManager.Instance.Modules.CmsKit().EnableAll();
            });
        }
    }
}
