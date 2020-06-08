using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.Quartz
{
    public class AbpQuartzModule : AbpModule
    {
        private IScheduler _scheduler;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var options = context.Services.ExecutePreConfiguredActions<AbpQuartzOptions>();
            context.Services.AddSingleton(AsyncHelper.RunSync(() => new StdSchedulerFactory(options.Properties).GetScheduler()));
            context.Services.AddSingleton(typeof(IJobFactory), typeof(AbpQuartzJobFactory));

            Configure<AbpQuartzOptions>(quartzOptions =>
            {
                quartzOptions.Properties = options.Properties;
                quartzOptions.StartDelay = options.StartDelay;
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var options = context.ServiceProvider.GetRequiredService<IOptions<AbpQuartzOptions>>().Value;

            _scheduler = context.ServiceProvider.GetService<IScheduler>();
            _scheduler.JobFactory = context.ServiceProvider.GetService<IJobFactory>();

            AsyncHelper.RunSync(() => options.StartSchedulerFactory.Invoke(_scheduler));
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            if (_scheduler.IsStarted)
            {
                AsyncHelper.RunSync(() => _scheduler.Shutdown());
            }
        }
    }
}