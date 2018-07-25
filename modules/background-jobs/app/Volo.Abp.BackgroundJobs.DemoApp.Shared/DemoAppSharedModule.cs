using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BackgroundJobs.DemoApp.Shared.Jobs;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs.DemoApp.Shared
{
    [DependsOn(
        typeof(AbpBackgroundJobsAbstractionsModule)
        )]
    public class DemoAppSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<DemoAppSharedModule>();
        }

        public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
            context.ServiceProvider
                .GetRequiredService<SampleJobCreator>()
                .CreateJobs();
        }
    }
}
