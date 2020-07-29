using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.Users.EntityFrameworkCore;
using Volo.CmsKit.Comments;
using Volo.CmsKit.Reactions;
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
                options.AddRepository<UserReaction, EfCoreUserReactionRepository>();
                options.AddRepository<Comment, EfCoreCommentRepository>();
                options.AddRepository<CmsUser, EfCoreCmsUserRepository>();
            });
        }
    }
}
