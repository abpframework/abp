using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BackgroundJobs.DemoApp.Shared.Jobs;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BackgroundJobs.DemoApp.Shared
{
    [DependsOn(typeof(AbpMultiTenancyModule))]
    public class DemoAppSharedModule : AbpModule
    {
        public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
            context.ServiceProvider
                .GetRequiredService<SampleJobCreator>()
                .CreateJobs();
        }
    }
}
