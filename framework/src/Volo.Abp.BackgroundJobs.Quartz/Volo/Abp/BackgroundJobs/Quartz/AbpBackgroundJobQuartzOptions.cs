using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Quartz;

namespace Volo.Abp.BackgroundJobs.Quartz;

public class AbpBackgroundJobQuartzOptions
{
    public int RetryCount { get; set; }

    public int RetryIntervalMillisecond { get; set; }


    [NotNull]
    public Func<int, IJobExecutionContext, JobExecutionException, Task> RetryStrategy {
        get => _retryStrategy;
        set => _retryStrategy = Check.NotNull(value, nameof(value));
    }
    private Func<int, IJobExecutionContext, JobExecutionException, Task> _retryStrategy;

    public AbpBackgroundJobQuartzOptions()
    {
        RetryCount = 3;
        RetryIntervalMillisecond = 3000;
        _retryStrategy = DefaultRetryStrategy;
    }

    private async Task DefaultRetryStrategy(int retryIndex, IJobExecutionContext executionContext, JobExecutionException exception)
    {
        exception.RefireImmediately = true;

        var retryCount = executionContext.JobDetail.JobDataMap.GetString(QuartzBackgroundJobManager.JobDataPrefix + nameof(RetryCount)).To<int>();
        if (retryIndex > retryCount)
        {
            exception.RefireImmediately = false;
            exception.UnscheduleAllTriggers = true;
            return;
        }

        var retryInterval = executionContext.JobDetail.JobDataMap.GetString(QuartzBackgroundJobManager.JobDataPrefix + nameof(RetryIntervalMillisecond)).To<int>();
        await Task.Delay(retryInterval);
    }
}
