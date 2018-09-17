using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs
{
    [DependsOn(
        typeof(BackgroundJobsDomainSharedModule),
        typeof(AbpBackgroundJobsModule),
        typeof(AbpAutoMapperModule)
        )]
    public class BackgroundJobsDomainModule : AbpModule //TODO: Rename to AbpBackgroundJobsDomainModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<BackgroundJobsDomainAutoMapperProfile>(validate: true);
            });
        }
    }
}
