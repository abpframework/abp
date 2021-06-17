using JetBrains.Annotations;
using Volo.Abp.GlobalFeatures;

namespace Volo.CmsKit.GlobalFeatures
{
    [GlobalFeatureName(Name)]
    public class MenuFeature : GlobalFeature
    {
        public const string Name = "CmsKit.Menus";

        internal MenuFeature(
            [NotNull] GlobalCmsKitFeatures cmsKit) : base(cmsKit)
        {

        }
    }
}
