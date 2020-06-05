using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;
using Volo.Abp.Users.MongoDB;
using Volo.Blogging.Blogs;
using Volo.Blogging.Comments;
using Volo.Blogging.Posts;
using Volo.Blogging.Tagging;
using Volo.Blogging.Users;

namespace Volo.Blogging.MongoDB
{
    [DependsOn(
        typeof(BloggingDomainModule),
        typeof(AbpMongoDbModule),
        typeof(AbpUsersMongoDbModule)
    )]
    public class BloggingMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<BloggingMongoDbContext>(options =>
            {
                options.AddRepository<Blog, MongoBlogRepository>();
                options.AddRepository<BlogUser, MongoBlogUserRepository>();
                options.AddRepository<Post, MongoPostRepository>();
                options.AddRepository<Tag, MongoTagRepository>();
                options.AddRepository<Comment, MongoCommentRepository>();
            });
        }
    }
}
