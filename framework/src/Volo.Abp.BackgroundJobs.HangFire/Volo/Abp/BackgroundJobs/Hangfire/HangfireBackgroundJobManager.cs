using System;
using System.Threading.Tasks;
using Hangfire;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs.Hangfire
{
    [Dependency(ReplaceServices = true)]
    public class HangfireBackgroundJobManager : IBackgroundJobManager, ITransientDependency
    {
        public virtual Task<string> EnqueueAsync<TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null)
        {
            return Task.FromResult(delay.HasValue
                ? BackgroundJob.Schedule<HangfireJobExecutionAdapter<TArgs>>(
                    adapter => adapter.ExecuteAsync(args),
                    delay.Value
                )
                : BackgroundJob.Enqueue<HangfireJobExecutionAdapter<TArgs>>(
                    adapter => adapter.ExecuteAsync(args)
                ));
        }
    }
}
