using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace Volo.Abp.Sms
{
    [DependsOn(
        typeof(AbpBackgroundJobsAbstractionsModule)
        )]
    public class AbpSmsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options =>
            {
                options.AddJob<BackgroundSmsSendingJob>();
            });
        }
    }
}
