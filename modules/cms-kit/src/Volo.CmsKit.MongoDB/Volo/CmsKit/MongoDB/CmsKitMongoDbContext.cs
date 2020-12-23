using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Ratings;
using Volo.CmsKit.Reactions;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.MongoDB
{
    [ConnectionStringName(CmsKitDbProperties.ConnectionStringName)]
    public class CmsKitMongoDbContext : AbpMongoDbContext, ICmsKitMongoDbContext
    {
        public IMongoCollection<Comment> Comments => Collection<Comment>();

        public IMongoCollection<UserReaction> UserReactions => Collection<UserReaction>();

        public IMongoCollection<CmsUser> CmsUsers => Collection<CmsUser>();

        public IMongoCollection<Rating> Ratings => Collection<Rating>();

        public IMongoCollection<Content> Contents => Collection<Content>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureCmsKit();
        }
    }
}
