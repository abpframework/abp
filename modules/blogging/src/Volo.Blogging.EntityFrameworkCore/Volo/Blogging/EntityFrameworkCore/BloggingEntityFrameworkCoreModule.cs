using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Blogging.Blogs;
using Volo.Blogging.Comments;
using Volo.Blogging.Posts;
using Volo.Blogging.Tagging;
using Volo.Blogging.Users;

namespace Volo.Blogging.EntityFrameworkCore
{
    [DependsOn(
        typeof(BloggingDomainModule),
        typeof(AbpEntityFrameworkCoreModule))]
    public class BloggingEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<BloggingDbContext>(options =>
            {
                options.AddRepository<Blog, EfCoreBlogRepository>();
                options.AddRepository<BlogUser, EfCoreBlogUserRepository>();
                options.AddRepository<Post, EfCorePostRepository>();
                options.AddRepository<Tag, EfCoreTagRepository>();
                options.AddRepository<Comment, EfCoreCommentRepository>();
            });
        }
    }
}
