using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs
{
    [DependsOn(
        typeof(BackgroundJobsDomainSharedModule),
        typeof(BackgroundJobsModule),
        typeof(AutoMapperModule)
        )]
    public class BackgroundJobsDomainModule : AbpModule //TODO: Rename to AbpBackgroundJobsDomainModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<BackgroundJobsDomainAutoMapperProfile>(validate: true);
            });
        }
    }
}
