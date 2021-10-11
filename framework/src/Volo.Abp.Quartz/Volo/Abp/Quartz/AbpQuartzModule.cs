using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.AdoJobStore.Common;
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
                // these are the defaults
                if (options.Properties[StdSchedulerFactory.PropertySchedulerJobFactoryType] == null)
                {
                    build.UseMicrosoftDependencyInjectionScopedJobFactory();
                }

                if (options.Properties[StdSchedulerFactory.PropertySchedulerTypeLoadHelperType] == null)
                {
                    build.UseSimpleTypeLoader();
                }

                if (options.Properties[StdSchedulerFactory.PropertyJobStoreType] == null)
                {
                    build.UseInMemoryStore();
                }

                if (options.Properties[StdSchedulerFactory.PropertyThreadPoolType] == null)
                {
                    build.UseDefaultThreadPool(tp =>
                    {
                        tp.MaxConcurrency = 10;
                    });
                }

                if (options.Properties["quartz.plugin.timeZoneConverter.type"] == null)
                {
                    build.UseTimeZoneConverter();
                }

                options.Configurator?.Invoke(build);
            });

            context.Services.AddSingleton(serviceProvider =>
            {
                return AsyncHelper.RunSync(() => serviceProvider.GetRequiredService<ISchedulerFactory>().GetScheduler());
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

            _scheduler = context.ServiceProvider.GetRequiredService<IScheduler>();

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
