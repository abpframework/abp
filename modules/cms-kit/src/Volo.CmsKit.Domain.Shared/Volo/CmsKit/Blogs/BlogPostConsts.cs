using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Blogs
{
    public static class BlogPostConsts
    {
        public static int MaxTitleLength { get; set; } = 64;

        public static int MaxSlugLength { get; set; } = 256;

        public static int MinSlugLength { get; set; } = 2;

        public static int MaxShortDescriptionLength { get; set; } = 256;

        public const string EntityType = "BlogPost";

        public const string CommentsFeatureName = CommentsFeature.Name;

        public const string ReactionsFeatureName = ReactionsFeature.Name;

        public const string RatingsFeatureName = RatingsFeature.Name;

        public const string TagsFeatureName = TagsFeature.Name;
    }
}
