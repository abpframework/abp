using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;
using Volo.Abp.BackgroundJobs;

namespace Volo.Docs.Admin
{
    [DependsOn(
        typeof(DocsDomainModule),
        typeof(DocsAdminApplicationContractsModule),
        typeof(AbpCachingModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpBackgroundJobsModule)
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

            Configure<AbpBackgroundJobWorkerOptions>(options =>
            {
                options.DefaultTimeout = 7200; //2 hours
            });
        }
    }
}
