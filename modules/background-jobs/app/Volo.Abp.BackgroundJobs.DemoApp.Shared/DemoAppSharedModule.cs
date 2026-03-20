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
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var dynamicJobManager = context.ServiceProvider.GetRequiredService<IDynamicBackgroundJobManager>();

            dynamicJobManager.RegisterHandler("CompileTimeAnonymousJob", (ctx, ct) =>
            {
                using (var doc = JsonDocument.Parse(ctx.JsonData))
                {
                    var value = doc.RootElement.TryGetProperty("value", out var prop)
                        ? prop.GetString()
                        : doc.RootElement.TryGetProperty("Value", out prop)
                            ? prop.GetString()
                            : null;
                    Console.WriteLine($"[ANONYMOUS-COMPILE] {value}");
                    return Task.CompletedTask;
                }
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
