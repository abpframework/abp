using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundJobs
{
    [DependsOn(
        typeof(AbpBackgroundJobsModule)
    )]
    public class AbpBackgroundJobsTestModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //TODO: Can we automatically register these!
            context.Services.Configure<BackgroundJobOptions>(options =>
            {
                options.JobTypes[MyJobArgs.Name] = typeof(MyJob);
            });

            context.Services.AddAssemblyOf<AbpBackgroundJobsTestModule>();
        }
    }
}