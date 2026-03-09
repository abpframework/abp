using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.BackgroundJobs.DemoApp.Shared.Jobs
{
    public class SampleJobCreator : ITransientDependency
    {
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IDynamicBackgroundJobHandlerProvider _dynamicBackgroundJobHandlerProvider;

        public SampleJobCreator(
            IBackgroundJobManager backgroundJobManager,
            IDynamicBackgroundJobHandlerProvider dynamicBackgroundJobHandlerProvider)
        {
            _backgroundJobManager = backgroundJobManager;
            _dynamicBackgroundJobHandlerProvider = dynamicBackgroundJobHandlerProvider;
        }

        public void CreateJobs()
        {
            AsyncHelper.RunSync(CreateJobsAsync);
        }

        public async Task CreateJobsAsync()
        {
            _dynamicBackgroundJobHandlerProvider.Register("RuntimeDynamicJob", context =>
            {
                context.Args.TryGetValue("Value", out var valueObj);
                Console.WriteLine($"[DYNAMIC-RUNTIME] {valueObj}");
                return Task.CompletedTask;
            });

            // Type-safe enqueue (existing)
            await _backgroundJobManager.EnqueueAsync(new WriteToConsoleGreenJobArgs { Value = "test 1 (green) - typed" });
            await _backgroundJobManager.EnqueueAsync(new WriteToConsoleYellowJobArgs { Value = "test 1 (yellow) - typed" });

            // String-based enqueue with strongly-typed args
            await _backgroundJobManager.EnqueueAsync(
                "GreenJob",
                (object)new WriteToConsoleGreenJobArgs { Value = "test 2 (green) - by name, typed args" }
            );
            await _backgroundJobManager.EnqueueAsync(
                "YellowJob",
                (object)new WriteToConsoleYellowJobArgs { Value = "test 2 (yellow) - by name, typed args" }
            );

            // String-based enqueue with anonymous object
            await _backgroundJobManager.EnqueueAsync(
                "GreenJob",
                (object)new { Value = "test 3 (green) - by name, anonymous", Time = DateTime.Now }
            );
            await _backgroundJobManager.EnqueueAsync(
                "YellowJob",
                (object)new { Value = "test 3 (yellow) - by name, anonymous", Time = DateTime.Now }
            );

            // Dynamic enqueue (compile-time and runtime handlers)
            if (!_backgroundJobManager.GetType().Name.Contains("RabbitMq", StringComparison.OrdinalIgnoreCase))
            {
                await _backgroundJobManager.EnqueueAsync(
                    "CompileTimeDynamicJob",
                    (object)new Dictionary<string, object> { ["Value"] = "test 4 (dynamic) - compile-time" }
                );
                await _backgroundJobManager.EnqueueAsync(
                    "RuntimeDynamicJob",
                    (object)new Dictionary<string, object> { ["Value"] = "test 5 (dynamic) - runtime" }
                );
            }
        }
    }
}
