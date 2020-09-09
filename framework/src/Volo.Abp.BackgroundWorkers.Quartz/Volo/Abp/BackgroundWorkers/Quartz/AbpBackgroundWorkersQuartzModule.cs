using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
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
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton(typeof(QuartzPeriodicBackgroundWorkerAdapter<>));
        }

        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            var options = context.ServiceProvider.GetService<IOptions<AbpBackgroundWorkerOptions>>().Value;
            if (!options.IsEnabled)
            {
                var quartzOptions = context.ServiceProvider.GetService<IOptions<AbpQuartzOptions>>().Value;
                quartzOptions.StartSchedulerFactory  = scheduler => Task.CompletedTask;
            }
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var quartzBackgroundWorkerOptions = context.ServiceProvider.GetService<IOptions<AbpBackgroundWorkerQuartzOptions>>().Value;
            if (quartzBackgroundWorkerOptions.IsAutoRegisterEnabled)
            {
                var backgroundWorkerManager = context.ServiceProvider.GetService<IBackgroundWorkerManager>();
                var works = context.ServiceProvider.GetServices<IQuartzBackgroundWorker>().Where(x=>x.AutoRegister);

                foreach (var work in works)
                {
                    backgroundWorkerManager.Add(work);
                }
            }
        }
    }
}
