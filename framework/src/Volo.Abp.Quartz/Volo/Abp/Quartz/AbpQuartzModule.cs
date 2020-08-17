using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
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

            context.Services.AddQuartz(options.Properties, build =>
            {
                build.UseMicrosoftDependencyInjectionScopedJobFactory();
                // these are the defaults
                build.UseSimpleTypeLoader();
                build.UseInMemoryStore();
                build.UseDefaultThreadPool(tp =>
                {
                    tp.MaxConcurrency = 10;
                });
                options.Configurator?.Invoke(build);
            });

            context.Services.AddSingleton(serviceProvider =>
            {
                return AsyncHelper.RunSync(() => serviceProvider.GetService<ISchedulerFactory>().GetScheduler());
            });

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
