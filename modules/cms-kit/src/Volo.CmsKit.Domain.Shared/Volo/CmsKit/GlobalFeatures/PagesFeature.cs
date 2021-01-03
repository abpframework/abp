using JetBrains.Annotations;
using Volo.Abp.GlobalFeatures;

namespace Volo.CmsKit.GlobalFeatures
{
    [GlobalFeatureName(Name)]
    public class PagesFeature:GlobalFeature
    {
        public const string Name = "CmsKit.Pages";

        internal PagesFeature(
            [NotNull] GlobalCmsKitFeatures cmsKit
        ) : base(cmsKit)
        {
            
        }

        public override void Enable()
        {
            var contentsFeature = FeatureManager.Modules.CmsKit().Contents;
            if (!contentsFeature.IsEnabled)
            {
                contentsFeature.Enable();
            }
            
            base.Enable();
        }
    }
}