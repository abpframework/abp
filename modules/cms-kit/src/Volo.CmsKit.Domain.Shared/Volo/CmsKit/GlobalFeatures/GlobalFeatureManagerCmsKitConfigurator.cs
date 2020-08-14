using Volo.Abp.GlobalFeatures;

namespace Volo.CmsKit.GlobalFeatures
{
    public class GlobalCmsKitFeatures : GlobalModuleFeatures
    {
        public const string ModuleName = "CmsKit";

        public ReactionsFeature Reactions => GetFeature<ReactionsFeature>(ReactionsFeature.Name);

        public CommentsFeature Comments => GetFeature<CommentsFeature>(CommentsFeature.Name);

        public GlobalCmsKitFeatures(GlobalFeatureManager featureManager)
            : base(featureManager)
        {
            AddFeature(new ReactionsFeature(this));
            AddFeature(new CommentsFeature(this));
        }
    }
}
