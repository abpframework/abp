using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundWorkers
{
    [DependsOn(
        typeof(AbpThreadingModule)
        )]
    public class AbpBackgroundWorkersModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<AbpBackgroundWorkersModule>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var options = context.ServiceProvider.GetRequiredService<IOptions<BackgroundWorkerOptions>>().Value;
            if (options.IsEnabled)
            {
                context.ServiceProvider
                    .GetRequiredService<IBackgroundWorkerManager>()
                    .Start();
            }
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            var options = context.ServiceProvider.GetRequiredService<IOptions<BackgroundWorkerOptions>>().Value;
            if (options.IsEnabled)
            {
                context.ServiceProvider
                    .GetRequiredService<IBackgroundWorkerManager>()
                    .StopAndWaitToStop();
            }
        }
    }
}
