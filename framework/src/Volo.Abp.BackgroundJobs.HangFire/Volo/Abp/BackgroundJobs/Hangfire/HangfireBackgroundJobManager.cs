using System;
using System.Threading.Tasks;
using Hangfire;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs.Hangfire
{
    [Dependency(ReplaceServices = true)]
    public class HangfireBackgroundJobManager : IBackgroundJobManager, ITransientDependency
    {
        public Task<string> EnqueueAsync<TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null)
        {
            if (!delay.HasValue)
            {
                return Task.FromResult(
                    BackgroundJob.Enqueue<HangfireJobExecutionAdapter<TArgs>>(
                        adapter => adapter.Execute(args)
                    )
                );
            }
            else
            {
                return Task.FromResult(
                    BackgroundJob.Schedule<HangfireJobExecutionAdapter<TArgs>>(
                        adapter => adapter.Execute(args),
                        delay.Value
                    )
                );
            }
        }
    }
}