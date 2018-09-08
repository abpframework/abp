using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.RabbitMQ;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs.RabbitMQ
{
    [DependsOn(
        typeof(AbpBackgroundJobsAbstractionsModule),
        typeof(AbpRabbitMqModule),
        typeof(AbpThreadingModule)
    )]
    public class AbpBackgroundJobsRabbitMqModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton(typeof(IJobQueue<>), typeof(JobQueue<>));
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            StartJobQueueManager(context);
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            StopJobQueueManager(context);
        }

        private static void StartJobQueueManager(ApplicationInitializationContext context)
        {
            context.ServiceProvider
                .GetRequiredService<IJobQueueManager>()
                .Start();
        }

        private static void StopJobQueueManager(ApplicationShutdownContext context)
        {
            context.ServiceProvider
                .GetRequiredService<IJobQueueManager>()
                .Stop();
        }
    }
}
