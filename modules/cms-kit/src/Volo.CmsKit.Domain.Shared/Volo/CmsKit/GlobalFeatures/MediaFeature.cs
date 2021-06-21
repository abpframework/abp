using JetBrains.Annotations;
using Volo.Abp.GlobalFeatures;

namespace Volo.CmsKit.GlobalFeatures
{
    [GlobalFeatureName(Name)]
    public class MediaFeature : GlobalFeature
    {
        public const string Name = "CmsKit.Medias";
        
        internal MediaFeature(
            [NotNull] GlobalCmsKitFeatures cmsKit
        ) : base(cmsKit)
        {
            
        }

        public override void Enable()
        {
            var userFeature = FeatureManager.Modules.CmsKit().User;
            if (!userFeature.IsEnabled)
            {
                userFeature.Enable();
            }

            base.Enable();
        }
    }
}