using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BackgroundJobs.DemoApp.Shared.Jobs;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BackgroundJobs.DemoApp.Shared
{
    [DependsOn(typeof(AbpMultiTenancyModule))]
    public class DemoAppSharedModule : AbpModule
    {
        public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
            var dynamicJobManager = context.ServiceProvider.GetRequiredService<IDynamicBackgroundJobManager>();

            dynamicJobManager.RegisterHandler("CompileTimeDynamicJob", (ctx, ct) =>
            {
                using (var doc = JsonDocument.Parse(ctx.JsonData))
                {
                    var value = doc.RootElement.TryGetProperty("value", out var prop)
                        ? prop.GetString()
                        : doc.RootElement.TryGetProperty("Value", out prop)
                            ? prop.GetString()
                            : null;
                    Console.WriteLine($"[DYNAMIC-COMPILE] {value}");
                    return Task.CompletedTask;
                }
            });

            context.ServiceProvider
                .GetRequiredService<SampleJobCreator>()
                .CreateJobs();

            await DynamicBackgroundWorkerDemoAsync(context);
        }

        private async Task DynamicBackgroundWorkerDemoAsync(ApplicationInitializationContext context)
        {
            var dynamicWorkerManager = context.ServiceProvider
                .GetService<IDynamicBackgroundWorkerManager>();

            if (dynamicWorkerManager == null)
            {
                return;
            }

            // AddAsync: Register a dynamic worker with a schedule and handler
            await dynamicWorkerManager.AddAsync(
                "DemoHeartbeatWorker",
                new DynamicBackgroundWorkerSchedule
                {
                    Period = 5000 //5 seconds
                },
                async (workerContext, cancellationToken) =>
                {
                    Console.WriteLine($"[{DateTime.Now}] DemoHeartbeatWorker executed.");
                    await Task.CompletedTask;
                }
            );

            // IsRegistered: Check if a dynamic worker is registered
            var isRegistered = dynamicWorkerManager.IsRegistered("DemoHeartbeatWorker");
            Console.WriteLine($"DemoHeartbeatWorker is registered: {isRegistered}");

            // UpdateScheduleAsync: Update the schedule of an existing dynamic worker
            var updated = await dynamicWorkerManager.UpdateScheduleAsync(
                "DemoHeartbeatWorker",
                new DynamicBackgroundWorkerSchedule
                {
                    Period = 10000 //Change to 10 seconds
                }
            );
            Console.WriteLine($"DemoHeartbeatWorker schedule updated: {updated}");

            // RemoveAsync: Remove a dynamic worker
            var removed = await dynamicWorkerManager.RemoveAsync("DemoHeartbeatWorker");
            Console.WriteLine($"DemoHeartbeatWorker removed: {removed}");

            // Re-add the worker to keep it running for demo purposes
            await dynamicWorkerManager.AddAsync(
                "DemoHeartbeatWorker",
                new DynamicBackgroundWorkerSchedule
                {
                    Period = 10000 //10 seconds
                },
                async (workerContext, cancellationToken) =>
                {
                    Console.WriteLine($"[{DateTime.Now}] DemoHeartbeatWorker executed.");
                    await Task.CompletedTask;
                }
            );
        }
    }
}
