using Volo.Abp;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Comments;
using Volo.CmsKit.GlobalResources;
using Volo.CmsKit.MediaDescriptors;
using Volo.CmsKit.Menus;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Ratings;
using Volo.CmsKit.Reactions;
using Volo.CmsKit.Tags;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.MongoDB;

public static class CmsKitMongoDbContextExtensions
{
    public static void ConfigureCmsKit(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<CmsUser>(x =>
        {
            x.CollectionName = CmsKitDbProperties.DbTablePrefix + "Users";
        });

        builder.Entity<UserReaction>(x =>
        {
            x.CollectionName = CmsKitDbProperties.DbTablePrefix + "UserReactions";
        });

        builder.Entity<Comment>(x =>
        {
            x.CollectionName = CmsKitDbProperties.DbTablePrefix + "Comments";
        });

        builder.Entity<Rating>(x =>
        {
            x.CollectionName = CmsKitDbProperties.DbTablePrefix + "Ratings";
        });

        builder.Entity<Tag>(x =>
        {
            x.CollectionName = CmsKitDbProperties.DbTablePrefix + "Tags";
        });

        builder.Entity<EntityTag>(x =>
        {
            x.CollectionName = CmsKitDbProperties.DbTablePrefix + "EntityTags";
        });

        builder.Entity<Page>(x =>
        {
            x.CollectionName = CmsKitDbProperties.DbTablePrefix + "Pages";
        });

        builder.Entity<Blog>(x =>
        {
            x.CollectionName = CmsKitDbProperties.DbTablePrefix + "Blogs";
        });

        builder.Entity<BlogPost>(x =>
        {
            x.CollectionName = CmsKitDbProperties.DbTablePrefix + "BlogPosts";
        });

        builder.Entity<BlogFeature>(x =>
        {
            x.CollectionName = CmsKitDbProperties.DbTablePrefix + "BlogFeatures";
        });

        builder.Entity<MediaDescriptor>(x =>
        {
            x.CollectionName = CmsKitDbProperties.DbTablePrefix + "MediaDescriptors";
        });

        builder.Entity<MenuItem>(x =>
        {
            x.CollectionName = CmsKitDbProperties.DbTablePrefix + "MenuItems";
        });

        builder.Entity<GlobalResource>(x =>
        {
            x.CollectionName = CmsKitDbProperties.DbTablePrefix + "GlobalResources";
        });
    }
}
