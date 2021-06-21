using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.BlobStoring;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;
using Volo.Blogging.Comments;
using Volo.Blogging.Posts;

namespace Volo.Blogging
{
    [DependsOn(
        typeof(BloggingDomainModule),
        typeof(BloggingApplicationContractsModule),
        typeof(AbpCachingModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpBlobStoringModule),
        typeof(AbpDddApplicationModule)
        )]
    public class BloggingApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<BloggingApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<BloggingApplicationAutoMapperProfile>(validate: true);
            });

            Configure<AuthorizationOptions>(options =>
            {
                //TODO: Rename UpdatePolicy/DeletePolicy since it's candidate to conflicts with other modules!
                options.AddPolicy("BloggingUpdatePolicy", policy => policy.Requirements.Add(CommonOperations.Update));
                options.AddPolicy("BloggingDeletePolicy", policy => policy.Requirements.Add(CommonOperations.Delete));
            });

            context.Services.AddSingleton<IAuthorizationHandler, CommentAuthorizationHandler>();
            context.Services.AddSingleton<IAuthorizationHandler, PostAuthorizationHandler>();

        }
    }
}
