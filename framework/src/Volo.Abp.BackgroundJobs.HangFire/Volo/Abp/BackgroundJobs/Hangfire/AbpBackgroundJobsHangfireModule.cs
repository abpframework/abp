using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Hangfire;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs.Hangfire
{
    [DependsOn(
        typeof(AbpBackgroundJobsAbstractionsModule),
        typeof(AbpHangfireModule)
        )]
    public class AbpBackgroundJobsHangfireModule : AbpModule
    {
        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            base.PostConfigureServices(context);

            var serviceProvider = context.Services.BuildServiceProvider();
            var backgroundJobOption = serviceProvider.GetRequiredService<IOptions<AbpBackgroundJobOptions>>();
            if (!backgroundJobOption.Value.IsJobExecutionEnabled)
            {
                Configure<AbpHangfireOptions>(options =>
                {
                    if (options.ServerOptions == null)
                    {
                        options.ServerOptions=new BackgroundJobServerOptions();
                    }
                    options.ServerOptions.Queues = new[] {options.EmptyQueueName};
                });
            }

        }
    }
}
