using JetBrains.Annotations;
using Volo.Abp.GlobalFeatures;

namespace Volo.CmsKit.GlobalFeatures
{
    [GlobalFeatureName(Name)]
    public class RatingsFeature : GlobalFeature
    {
        public const string Name = "CmsKit.Ratings";

        internal RatingsFeature(
            [NotNull] GlobalCmsKitFeatures cmsKit
        ) : base(cmsKit)
        {
            
        }
    }
}