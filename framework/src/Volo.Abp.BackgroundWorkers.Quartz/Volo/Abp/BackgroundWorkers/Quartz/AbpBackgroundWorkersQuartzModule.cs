using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;
using Volo.Abp.Quartz;

namespace Volo.Abp.BackgroundWorkers.Quartz
{
    [DependsOn(
        typeof(AbpBackgroundWorkersModule),
        typeof(AbpQuartzModule)
    )]
    public class AbpBackgroundWorkersQuartzModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddConventionalRegistrar(new AbpQuartzConventionalRegistrar());
            context.Services.Replace(ServiceDescriptor
                .Transient<IBackgroundWorkerManager, QuartzBackgroundWorkerManager>());

        }


        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var backgroundWorkerManager = scope.ServiceProvider.GetService<IBackgroundWorkerManager>();
                var works = scope.ServiceProvider.GetServices<IQuartzBackgroundWorker>();

                foreach (var work in works)
                {
                    backgroundWorkerManager.Add(work);
                }
            }
        }
    }
}
