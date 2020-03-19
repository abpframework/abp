using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Modularity;
using Volo.Blogging.Blogs;
using Volo.Blogging.Comments;
using Volo.Blogging.Posts;
using Volo.Blogging.Tagging;

namespace Volo.Blogging
{
    [DependsOn(
        typeof(BloggingDomainSharedModule),
        typeof(AbpDddDomainModule))]
    public class BloggingDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpDistributedEventBusOptions>(options =>
            {
                options.EtoMappings.Add<Blog, BlogEto>();
                options.EtoMappings.Add<Comment, CommentEto>();
                options.EtoMappings.Add<Post, PostEto>();
                options.EtoMappings.Add<Tag, TagEto>();
            });

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<BloggingDomainMappingProfile>(validate: true);
            });
        }
    }
}
