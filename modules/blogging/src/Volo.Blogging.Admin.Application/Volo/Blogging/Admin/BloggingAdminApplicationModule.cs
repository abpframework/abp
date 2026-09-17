using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Mapperly;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace Volo.Blogging.Admin
{
    [DependsOn(
        typeof(BloggingDomainModule),
        typeof(BloggingAdminApplicationContractsModule),
        typeof(AbpCachingModule),
        typeof(AbpMapperlyModule),
        typeof(AbpDddApplicationModule)
        )]
    public class BloggingAdminApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMapperlyObjectMapper<BloggingAdminApplicationModule>();
        }
    }
}
