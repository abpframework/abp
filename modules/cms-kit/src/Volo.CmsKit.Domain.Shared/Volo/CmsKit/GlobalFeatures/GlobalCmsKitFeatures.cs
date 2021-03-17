using JetBrains.Annotations;
using Volo.Abp.GlobalFeatures;

namespace Volo.CmsKit.GlobalFeatures
{
    public class GlobalCmsKitFeatures : GlobalModuleFeatures
    {
        public const string ModuleName = "CmsKit";

        public ReactionsFeature Reactions => GetFeature<ReactionsFeature>();

        public CommentsFeature Comments => GetFeature<CommentsFeature>();
        
        public MediaFeature Media => GetFeature<MediaFeature>();

        public RatingsFeature Ratings => GetFeature<RatingsFeature>();

        public TagsFeature Tags => GetFeature<TagsFeature>();

        public PagesFeature Pages => GetFeature<PagesFeature>();

        public CmsUserFeature User => GetFeature<CmsUserFeature>();

        public GlobalCmsKitFeatures([NotNull] GlobalFeatureManager featureManager)
            : base(featureManager)
        {
            AddFeature(new ReactionsFeature(this));
            AddFeature(new MediaFeature(this));
            AddFeature(new CommentsFeature(this));
            AddFeature(new RatingsFeature(this));
            AddFeature(new TagsFeature(this));
            AddFeature(new PagesFeature(this));
            AddFeature(new BlogsFeature(this));
            AddFeature(new CmsUserFeature(this));
        }
    }
}
