using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Ratings;
using Volo.CmsKit.Reactions;
using Volo.CmsKit.Tags;
using Volo.CmsKit.Users;
using Tag = MongoDB.Driver.Tag;

namespace Volo.CmsKit.MongoDB
{
    [ConnectionStringName(CmsKitDbProperties.ConnectionStringName)]
    public interface ICmsKitMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<UserReaction> UserReactions { get; }

        IMongoCollection<Comment> Comments { get; }

        IMongoCollection<CmsUser> CmsUsers { get; }
        
        IMongoCollection<Rating> Ratings { get; }
        
        IMongoCollection<Content> Contents { get; }
        
        IMongoCollection<Tag> Tags { get; }
        
        IMongoCollection<EntityTag> EntityTags { get; }
        
        IMongoCollection<Page> Pages { get; }
    }
}
