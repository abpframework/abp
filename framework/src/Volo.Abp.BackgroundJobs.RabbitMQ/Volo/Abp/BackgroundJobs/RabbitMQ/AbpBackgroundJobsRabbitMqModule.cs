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

            context.Services.AddAssemblyOf<AbpBackgroundJobsRabbitMqModule>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            context.ServiceProvider
                .GetRequiredService<IJobQueueManager>()
                .Start();
        }
    }
}
