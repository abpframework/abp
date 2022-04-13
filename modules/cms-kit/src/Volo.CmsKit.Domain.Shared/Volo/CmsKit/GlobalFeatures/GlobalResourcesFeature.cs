using JetBrains.Annotations;
using Volo.Abp.GlobalFeatures;

namespace Volo.CmsKit.GlobalFeatures;

[GlobalFeatureName(Name)]
public class GlobalResourcesFeature : GlobalFeature
{
    public const string Name = "CmsKit.GlobalResources";

    internal GlobalResourcesFeature(
        [NotNull] GlobalCmsKitFeatures cmsKit
    ) : base(cmsKit)
    {
    }
}