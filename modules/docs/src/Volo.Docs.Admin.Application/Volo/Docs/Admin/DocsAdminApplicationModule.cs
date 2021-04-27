using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace Volo.Docs.Admin
{
    [DependsOn(
        typeof(DocsDomainModule),
        typeof(DocsAdminApplicationContractsModule),
        typeof(AbpCachingModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpDddApplicationModule)
    )]
    public class DocsAdminApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<DocsAdminApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<DocsAdminApplicationAutoMapperProfile>(validate: true);
            });
        }
    }
}
