using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Users.EntityFrameworkCore;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Ratings;
using Volo.CmsKit.Reactions;
using Volo.CmsKit.Tags;
using Volo.CmsKit.Users;

namespace Volo.CmsKit.EntityFrameworkCore
{
    [DependsOn(
        typeof(CmsKitDomainModule),
        typeof(AbpUsersEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class CmsKitEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<CmsKitDbContext>(options =>
            {
                options.AddRepository<CmsUser, EfCoreCmsUserRepository>();
                options.AddRepository<UserReaction, EfCoreUserReactionRepository>();
                options.AddRepository<Comment, EfCoreCommentRepository>();
                options.AddRepository<Rating, EfCoreRatingRepository>();
                options.AddRepository<Content, EfCoreContentRepository>();
                options.AddRepository<Tag, EfCoreTagRepository>();
                options.AddRepository<EntityTag, EfCoreEntityTagRepository>();
                options.AddRepository<Page, EfCorePageRepository>();
            });
        }
    }
}
