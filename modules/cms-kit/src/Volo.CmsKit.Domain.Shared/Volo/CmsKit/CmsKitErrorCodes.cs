namespace Volo.CmsKit
{
    public static class CmsKitErrorCodes
    {
        public static class Tags
        {
            public const string TagAlreadyExist = "CmsKit:Tag:0001";
            public const string EntityNotTaggable = "CmsKit:Tag:0002";
        }

        public static class Pages
        {
            public const string SlugAlreadyExist = "CmsKit:Page:0001";
        }

        public static class Ratings
        {
            public const string EntityCantHaveRating = "CmsKit:Rating:0001";
        }

        public static class Reactions
        {
            public const string EntityCantHaveReaction = "CmsKit:Reaction:0001";
        }

        public static class Blogs
        {
            public const string SlugAlreadyExists = "CmsKit:Blog:0001";
        }
        
        public static class BlogPosts
        {
            public const string SlugAlreadyExist = "CmsKit:BlogPost:0001";
        }

        public static class Comments
        {
            public const string EntityNotCommentable = "CmsKit:Comments:0001";
        }
        
        public static class MediaDescriptors
        {
            public const string InvalidName = "CmsKit:Media:0001";
            public const string EntityTypeDoesntExist = "CmsKit:Media:0002";
        }
    }
}
