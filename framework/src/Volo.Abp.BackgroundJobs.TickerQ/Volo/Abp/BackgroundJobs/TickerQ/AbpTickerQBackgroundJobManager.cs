using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TickerQ.Utilities;
using TickerQ.Utilities.Interfaces.Managers;
using TickerQ.Utilities.Models.Ticker;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs.TickerQ;

[Dependency(ReplaceServices = true)]
public class AbpTickerQBackgroundJobManager : IBackgroundJobManager, ITransientDependency
{
    protected ITimeTickerManager<TimeTicker> TimeTickerManager { get; }
    protected AbpBackgroundJobOptions Options { get; }
    protected AbpBackgroundJobsTickerQOptions TickerQOptions { get; }

    public AbpTickerQBackgroundJobManager(
        ITimeTickerManager<TimeTicker> timeTickerManager,
        IOptions<AbpBackgroundJobOptions> options,
        IOptions<AbpBackgroundJobsTickerQOptions> tickerQOptions)
    {
        TimeTickerManager = timeTickerManager;
        Options = options.Value;
        TickerQOptions = tickerQOptions.Value;
    }

    public virtual async Task<string> EnqueueAsync<TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal, TimeSpan? delay = null)
    {
        var job = Options.GetJob(typeof(TArgs));
        var timeTicker = new TimeTicker
        {
            Id = Guid.NewGuid(),
            Function = job.JobName,
            ExecutionTime = delay == null ? DateTime.UtcNow : DateTime.UtcNow.Add(delay.Value),
            Request = TickerHelper.CreateTickerRequest<TArgs>(args),
        };

        var config = TickerQOptions.GetConfigurationOrNull(job.JobType);
        if (config != null)
        {
            timeTicker.Retries = config.Retries ?? timeTicker.Retries;
            timeTicker.RetryIntervals = config.RetryIntervals ?? timeTicker.RetryIntervals;
            timeTicker.BatchParent = config.BatchParent ?? timeTicker.BatchParent;
            timeTicker.BatchRunCondition = config.BatchRunCondition ?? timeTicker.BatchRunCondition;
        }

        var result = await TimeTickerManager.AddAsync(timeTicker);
        return !result.IsSucceded ? timeTicker.Id.ToString() : result.Result.Id.ToString();
    }
}
