using JetBrains.Annotations;
using Volo.Abp.GlobalFeatures;

namespace Volo.CmsKit.GlobalFeatures
{
    public class GlobalCmsKitFeatures : GlobalModuleFeatures
    {
        public const string ModuleName = "CmsKit";

        public ReactionsFeature Reactions => GetFeature<ReactionsFeature>();

        public CommentsFeature Comments => GetFeature<CommentsFeature>();

        public GlobalCmsKitFeatures([NotNull] GlobalFeatureManager featureManager)
            : base(featureManager)
        {
            AddFeature(new ReactionsFeature(this));
            AddFeature(new CommentsFeature(this));
        }
    }
}
