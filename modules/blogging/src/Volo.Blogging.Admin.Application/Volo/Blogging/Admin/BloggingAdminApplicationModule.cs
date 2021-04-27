using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;
using Volo.Blogging.Comments;
using Volo.Blogging.Posts;

namespace Volo.Blogging.Admin
{
    [DependsOn(
        typeof(BloggingDomainModule),
        typeof(BloggingAdminApplicationContractsModule),
        typeof(AbpCachingModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpDddApplicationModule)
        )]
    public class BloggingAdminApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<BloggingAdminApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<BloggingAdminApplicationAutoMapperProfile>(validate: true);
            });
        }
    }
}
