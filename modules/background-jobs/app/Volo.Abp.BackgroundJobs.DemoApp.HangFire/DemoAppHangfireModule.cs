using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs.DemoApp.Shared;
using Volo.Abp.Modularity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Volo.Abp.BackgroundJobs.Hangfire;

namespace Volo.Abp.BackgroundJobs.DemoApp.HangFire
{
    [DependsOn(
        typeof(DemoAppSharedModule),
        typeof(AbpAutofacModule),
        typeof(AbpBackgroundJobsHangfireModule)
    )]
    public class DemoAppHangfireModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = ConfigurationHelper.BuildConfiguration();
            context.Services.AddConfiguration(configuration);

            context.Services.PreConfigure<IGlobalConfiguration>(hangfireConfiguration =>
            {
                hangfireConfiguration.UseSqlServerStorage(configuration.GetConnectionString("Default"));
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            context
                .ServiceProvider
                .GetRequiredService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);
        }
    }
}
