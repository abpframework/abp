using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using Volo.Abp.Modularity;

namespace Volo.Abp.Quartz
{
    public class AbpQuartzModule : AbpModule
    {

        private IScheduler _scheduler;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var options = context.Services.ExecutePreConfiguredActions<AbpQuartzPreOptions>();
            context.Services.AddQuartz(options);
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            _scheduler = context.ServiceProvider.GetService<IScheduler>();
            _scheduler.JobFactory = context.ServiceProvider.GetService<IJobFactory>();
            _scheduler.Start();
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            //TODO: ABP may provide two methods for application shutdown: OnPreApplicationShutdown & OnApplicationShutdown
            _scheduler.Shutdown();
        }
    }
}
