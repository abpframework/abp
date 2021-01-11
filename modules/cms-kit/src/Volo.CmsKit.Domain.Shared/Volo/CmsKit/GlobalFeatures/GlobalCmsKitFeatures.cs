using JetBrains.Annotations;
using Volo.Abp.GlobalFeatures;

namespace Volo.CmsKit.GlobalFeatures
{
    public class GlobalCmsKitFeatures : GlobalModuleFeatures
    {
        public const string ModuleName = "CmsKit";

        public ReactionsFeature Reactions => GetFeature<ReactionsFeature>();

        public CommentsFeature Comments => GetFeature<CommentsFeature>();

        public RatingsFeature Ratings => GetFeature<RatingsFeature>();

        public TagsFeature Tags => GetFeature<TagsFeature>();

        public ContentsFeature Contents => GetFeature<ContentsFeature>();

        public PagesFeature Pages => GetFeature<PagesFeature>();

        public GlobalCmsKitFeatures([NotNull] GlobalFeatureManager featureManager)
            : base(featureManager)
        {
            AddFeature(new ReactionsFeature(this));
            AddFeature(new CommentsFeature(this));
            AddFeature(new RatingsFeature(this));
            AddFeature(new TagsFeature(this));
            AddFeature(new ContentsFeature(this));
            AddFeature(new PagesFeature(this));
        }
    }
}
