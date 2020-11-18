using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Ratings;
using Volo.CmsKit.Reactions;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.MongoDB
{
    [ConnectionStringName(CmsKitDbProperties.ConnectionStringName)]
    public interface ICmsKitMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<UserReaction> UserReactions { get; }

        IMongoCollection<Comment> Comments { get; }

        IMongoCollection<CmsUser> CmsUsers { get; }
        
        IMongoCollection<Rating> Ratings { get; }
    }
}
