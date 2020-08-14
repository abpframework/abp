using JetBrains.Annotations;
using Volo.Abp.GlobalFeatures;

namespace Volo.CmsKit.GlobalFeatures
{
    [GlobalFeatureName(Name)]
    public class ReactionsFeature : GlobalFeature
    {
        public const string Name = "CmsKit.Reactions";

        internal  ReactionsFeature(
            [NotNull] GlobalCmsKitFeatures cmsKit
            ) : base(cmsKit)
        {
        }
    }
}
