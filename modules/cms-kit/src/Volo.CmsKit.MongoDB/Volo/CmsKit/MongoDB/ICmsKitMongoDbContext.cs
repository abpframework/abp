using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Comments;
using Volo.CmsKit.MediaDescriptors;
using Volo.CmsKit.Menus;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Ratings;
using Volo.CmsKit.Reactions;
using Volo.CmsKit.Tags;
using Volo.CmsKit.Users;
using Tag = Volo.CmsKit.Tags.Tag;

namespace Volo.CmsKit.MongoDB
{
    [ConnectionStringName(CmsKitDbProperties.ConnectionStringName)]
    public interface ICmsKitMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<UserReaction> UserReactions { get; }

        IMongoCollection<Comment> Comments { get; }

        IMongoCollection<CmsUser> CmsUsers { get; }

        IMongoCollection<Rating> Ratings { get; }

        IMongoCollection<Tag> Tags { get; }

        IMongoCollection<EntityTag> EntityTags { get; }

        IMongoCollection<Page> Pages { get; }

        IMongoCollection<Blog> Blogs { get; }

        IMongoCollection<BlogPost> BlogPosts { get; }

        IMongoCollection<BlogFeature> BlogFeatures { get; }
        
        IMongoCollection<MediaDescriptor> MediaDescriptors { get; }

        IMongoCollection<MenuItem> Menus { get; }
    }
}
