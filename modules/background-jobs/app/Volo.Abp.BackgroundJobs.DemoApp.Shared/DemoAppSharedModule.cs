using System;
using System.Text.Json;
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
                options.AddAnonymousJobHandler("CompileTimeAnonymousJob", (jsonData, sp, ct) =>
                {
                    var doc = JsonDocument.Parse(jsonData);
                    var value = doc.RootElement.TryGetProperty("Value", out var prop) ? prop.GetString() : null;
                    Console.WriteLine($"[ANONYMOUS-COMPILE] {value}");
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
