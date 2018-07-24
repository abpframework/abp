using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs.DemoApp
{
    [DependsOn(
        typeof(BackgroundJobsEntityFrameworkCoreModule),
        typeof(AbpAutofacModule)
        )]
    public class DemoAppModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<DemoAppModule>();
        }
    }
}
