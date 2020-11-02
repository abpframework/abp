using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace Volo.Abp.Sms
{
    public class AbpSmsModule : AbpModule
    {
        [DependsOn(
            typeof(AbpBackgroundJobsAbstractionsModule)
            )]
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options =>
            {
                options.AddJob<BackgroundSmsSendingJob>();
            });
        }
    }
}
