using JetBrains.Annotations;
using Volo.Abp.GlobalFeatures;

namespace Volo.CmsKit.GlobalFeatures;

[GlobalFeatureName(Name)]
public class RobotsTxtFeature : GlobalFeature
{
    public const string Name = "CmsKit.RobotsTxt";

    internal RobotsTxtFeature(
        [NotNull] GlobalCmsKitFeatures cmsKit
    ) : base(cmsKit)
    {
    }
}
