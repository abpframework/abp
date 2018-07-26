using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Modularity;
using Volo.Abp.RabbitMQ;

namespace Volo.Abp.BackgroundJobs.RabbitMQ
{
    [DependsOn(
        typeof(AbpBackgroundJobsAbstractionsModule),
        typeof(AbpRabbitMqModule)
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
            //TODO: Move all to another class and stop listeners when needed!
            var options = context.ServiceProvider.GetRequiredService<IOptions<BackgroundJobOptions>>().Value;
            if (options.IsJobExecutionEnabled)
            {
                foreach (var jobType in options.JobTypes.Values)
                {
                    var jobListener = (IJobListener)context.ServiceProvider.GetRequiredService(typeof(JobListener<>).MakeGenericType(jobType));
                    jobListener.Start();
                }
            }
        }
    }
}
