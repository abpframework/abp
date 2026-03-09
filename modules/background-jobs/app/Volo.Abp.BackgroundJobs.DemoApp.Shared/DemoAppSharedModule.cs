using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BackgroundJobs.DemoApp.Shared.Jobs;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BackgroundJobs.DemoApp.Shared
{
    [DependsOn(typeof(AbpMultiTenancyModule))]
    public class DemoAppSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options =>
            {
                options.AddDynamicJob("CompileTimeDynamicJob", dynamicContext =>
                {
                    dynamicContext.Args.TryGetValue("Value", out var valueObj);
                    var value = valueObj?.ToString();
                    Console.WriteLine($"[DYNAMIC-COMPILE] {value}");
                    return Task.CompletedTask;
                });
            });
        }

        public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
            context.ServiceProvider
                .GetRequiredService<SampleJobCreator>()
                .CreateJobs();
        }
    }
}
