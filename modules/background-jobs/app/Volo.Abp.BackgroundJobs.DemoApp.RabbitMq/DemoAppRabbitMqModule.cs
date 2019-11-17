using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs.DemoApp.Shared;
using Volo.Abp.BackgroundJobs.RabbitMQ;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs.DemoApp.RabbitMq
{
    [DependsOn(
        typeof(DemoAppSharedModule),
        typeof(AbpAutofacModule),
        typeof(AbpBackgroundJobsRabbitMqModule)
    )]
    public class DemoAppRabbitMqModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            //TODO: Configure console logging
            //context
            //    .ServiceProvider
            //    .GetRequiredService<ILoggerFactory>()
            //    .AddConsole(LogLevel.Debug);
        }
    }
}
