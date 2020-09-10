using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;
using Volo.Abp.Users.MongoDB;
using Volo.CmsKit.Comments;
using Volo.CmsKit.MongoDB.Comments;
using Volo.CmsKit.MongoDB.Ratings;
using Volo.CmsKit.MongoDB.Reactions;
using Volo.CmsKit.MongoDB.Users;
using Volo.CmsKit.Ratings;
using Volo.CmsKit.Reactions;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.MongoDB
{
    [DependsOn(
        typeof(CmsKitDomainModule),
        typeof(AbpUsersMongoDbModule),
        typeof(AbpMongoDbModule)
        )]
    public class CmsKitMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<CmsKitMongoDbContext>(options =>
            {
                options.AddRepository<CmsUser, MongoCmsUserRepository>();
                options.AddRepository<UserReaction, MongoUserReactionRepository>();
                options.AddRepository<Comment, MongoCommentRepository>();
                options.AddRepository<Rating, MongoRatingRepository>();
            });
        }
    }
}
