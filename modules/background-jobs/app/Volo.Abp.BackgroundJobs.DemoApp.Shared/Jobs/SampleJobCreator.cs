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
        private readonly IAnonymousJobHandlerRegistry _anonymousJobHandlerRegistry;

        public SampleJobCreator(
            IBackgroundJobManager backgroundJobManager,
            IAnonymousJobHandlerRegistry anonymousJobHandlerRegistry)
        {
            _backgroundJobManager = backgroundJobManager;
            _anonymousJobHandlerRegistry = anonymousJobHandlerRegistry;
        }

        public void CreateJobs()
        {
            AsyncHelper.RunSync(CreateJobsAsync);
        }

        public async Task CreateJobsAsync()
        {
            _anonymousJobHandlerRegistry.Register("RuntimeAnonymousJob", (jsonData, sp, ct) =>
            {
                var doc = JsonDocument.Parse(jsonData);
                var value = doc.RootElement.TryGetProperty("Value", out var prop) ? prop.GetString() : null;
                Console.WriteLine($"[ANONYMOUS-RUNTIME] {value}");
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

            // Anonymous job enqueue (compile-time and runtime handlers)
            if (!_backgroundJobManager.GetType().Name.ToUpperInvariant().Contains("RABBITMQ"))
            {
                await _backgroundJobManager.EnqueueAsync(
                    "CompileTimeAnonymousJob",
                    new { Value = "test 4 (anonymous) - compile-time" }
                );
                await _backgroundJobManager.EnqueueAsync(
                    "RuntimeAnonymousJob",
                    new { Value = "test 5 (anonymous) - runtime" }
                );
            }
        }
    }
}
