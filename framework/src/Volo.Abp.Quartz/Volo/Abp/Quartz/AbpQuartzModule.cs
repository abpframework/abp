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

            // TODO: Remove this once Pomelo update MySqlConnector to >= 1.0.0 : https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/pull/1103
            AdaptMysqlConnector();

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

        private void AdaptMysqlConnector()
        {
            var mySqlAvailable = System.Type.GetType("MySql.Data.MySqlClient.MySqlConnection, MySqlConnector") != null;
            if (mySqlAvailable)
            {
                // Overriding the default 'MySqlConnector' provider to use the old 'MySql.Data' namespace found in MySqlConnector < 1.0.0
                DbProvider.RegisterDbMetadata("MySqlConnector", new DbMetadata()
                {
                    ProductName = "MySQL, MySqlConnector provider",
                    AssemblyName = "MySqlConnector",
                    ConnectionType = System.Type.GetType("MySql.Data.MySqlClient.MySqlConnection, MySqlConnector"),
                    CommandType = System.Type.GetType("MySql.Data.MySqlClient.MySqlCommand, MySqlConnector"),
                    ParameterType = System.Type.GetType("MySql.Data.MySqlClient.MySqlParameter, MySqlConnector"),
                    ParameterDbType = System.Type.GetType("MySql.Data.MySqlClient.MySqlDbType, MySqlConnector"),
                    ParameterDbTypePropertyName = "MySqlDbType",
                    ParameterNamePrefix = "?",
                    ExceptionType = System.Type.GetType("MySql.Data.MySqlClient.MySqlException, MySqlConnector"),
                    BindByName = true,
                    DbBinaryTypeName = "Blob"
                });
            }
        }
    }
}
