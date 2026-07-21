using System;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs.DemoApp.Shared.Jobs
{
    public class SampleJobCreator : ITransientDependency
    {
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IDynamicBackgroundJobManager _dynamicBackgroundJobManager;

        public SampleJobCreator(
            IBackgroundJobManager backgroundJobManager,
            IDynamicBackgroundJobManager dynamicBackgroundJobManager)
        {
            _backgroundJobManager = backgroundJobManager;
            _dynamicBackgroundJobManager = dynamicBackgroundJobManager;
        }

        public void CreateJobs()
        {
            AsyncHelper.RunSync(CreateJobsAsync);
        }

        public async Task CreateJobsAsync()
        {
            // Type-safe enqueue (existing)
            await _backgroundJobManager.EnqueueAsync(new WriteToConsoleGreenJobArgs { Value = "test 1 (green) - typed" });
            await _backgroundJobManager.EnqueueAsync(new WriteToConsoleYellowJobArgs { Value = "test 1 (yellow) - typed" });

            // Register runtime dynamic handler
            _dynamicBackgroundJobManager.RegisterHandler("RuntimeDynamicJob", (context, ct) =>
            {
                using (var doc = JsonDocument.Parse(context.JsonData))
                {
                    var value = doc.RootElement.TryGetProperty("value", out var prop)
                        ? prop.GetString()
                        : doc.RootElement.TryGetProperty("Value", out prop)
                            ? prop.GetString()
                            : null;
                    Console.WriteLine($"[DYNAMIC-RUNTIME] {value}");
                    return Task.CompletedTask;
                }
            });

            // String-based enqueue with typed job (by name)
            await _dynamicBackgroundJobManager.EnqueueAsync(
                "GreenJob",
                new WriteToConsoleGreenJobArgs { Value = "test 2 (green) - by name, typed args" }
            );
            await _dynamicBackgroundJobManager.EnqueueAsync(
                "YellowJob",
                new WriteToConsoleYellowJobArgs { Value = "test 2 (yellow) - by name, typed args" }
            );

            // String-based enqueue with anonymous object (typed job path)
            await _dynamicBackgroundJobManager.EnqueueAsync(
                "GreenJob",
                new { Value = "test 3 (green) - by name, dynamic", Time = DateTime.Now }
            );
            await _dynamicBackgroundJobManager.EnqueueAsync(
                "YellowJob",
                new { Value = "test 3 (yellow) - by name, dynamic", Time = DateTime.Now }
            );

            // Dynamic job handlers
            await _dynamicBackgroundJobManager.EnqueueAsync(
                "CompileTimeDynamicJob",
                new { Value = "test 4 (dynamic) - compile-time" }
            );
            await _dynamicBackgroundJobManager.EnqueueAsync(
                "RuntimeDynamicJob",
                new { Value = "test 5 (dynamic) - runtime" }
            );
        }
    }
}
