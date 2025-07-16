using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;
using Volo.Docs.Common;
using Volo.Abp.BackgroundJobs;

namespace Volo.Docs.Admin
{
    [DependsOn(
        typeof(DocsDomainModule),
        typeof(DocsAdminApplicationContractsModule),
        typeof(DocsCommonApplicationModule),
        typeof(AbpCachingModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpBackgroundJobsAbstractionsModule)
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
