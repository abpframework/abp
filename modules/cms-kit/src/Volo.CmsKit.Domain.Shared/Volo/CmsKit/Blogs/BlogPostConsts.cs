namespace Volo.CmsKit.Blogs
{
    public static class BlogPostConsts
    {
        public static int MaxTitleLength { get; set; } = 64;

        public static int MaxUrlSlugLength { get; set; } = 256;

        public static int MinUrlSlugLength { get; set; } = 2;

        public static int MaxShortDescriptionLength { get; set; } = 256;

        public const string EntityType = "BlogPost";
    }
}
