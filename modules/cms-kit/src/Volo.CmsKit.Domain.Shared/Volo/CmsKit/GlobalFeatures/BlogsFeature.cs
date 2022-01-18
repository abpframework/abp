using JetBrains.Annotations;
using Volo.Abp.GlobalFeatures;

namespace Volo.CmsKit.GlobalFeatures;

[GlobalFeatureName(Name)]
public class BlogsFeature : GlobalFeature
{
    public const string Name = "CmsKit.Blogs";

    internal BlogsFeature(
        [NotNull] GlobalCmsKitFeatures cmsKit
    ) : base(cmsKit)
    {
    }
}
