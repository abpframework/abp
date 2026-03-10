using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TickerQ.Utilities;
using TickerQ.Utilities.Entities;
using TickerQ.Utilities.Interfaces.Managers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Abp.BackgroundJobs.TickerQ;

[Dependency(ReplaceServices = true)]
public class AbpTickerQBackgroundJobManager : IBackgroundJobManager, ITransientDependency
{
    private readonly static MethodInfo CreateTickerRequestMethod = typeof(TickerHelper).GetMethod(nameof(TickerHelper.CreateTickerRequest), BindingFlags.Public | BindingFlags.Static)!;
    
    protected ITimeTickerManager<TimeTickerEntity> TimeTickerManager { get; }
    protected AbpBackgroundJobOptions Options { get; }
    protected AbpBackgroundJobsTickerQOptions TickerQOptions { get; }
    protected IAnonymousJobHandlerRegistry AnonymousJobHandlerRegistry { get; }
    protected IJsonSerializer JsonSerializer { get; }

    public AbpTickerQBackgroundJobManager(
        ITimeTickerManager<TimeTickerEntity> timeTickerManager,
        IOptions<AbpBackgroundJobOptions> options,
        IOptions<AbpBackgroundJobsTickerQOptions> tickerQOptions,
        IAnonymousJobHandlerRegistry anonymousJobHandlerRegistry,
        IJsonSerializer jsonSerializer)
    {
        TimeTickerManager = timeTickerManager;
        Options = options.Value;
        TickerQOptions = tickerQOptions.Value;
        AnonymousJobHandlerRegistry = anonymousJobHandlerRegistry;
        JsonSerializer = jsonSerializer;
    }

    public virtual async Task<string> EnqueueAsync<TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal, TimeSpan? delay = null)
    {
        var job = Options.GetJob(typeof(TArgs));
        return await EnqueueAsync(job, args!, priority, delay);
    }

    public virtual async Task<string> EnqueueAsync(string jobName, object args, BackgroundJobPriority priority = BackgroundJobPriority.Normal, TimeSpan? delay = null)
    {
        if (ShouldWrapAsAnonymousJob(jobName))
        {
            var jsonData = JsonSerializer.Serialize(args);
            var anonymousArgs = new AnonymousJobArgs(jobName, jsonData);
            return await EnqueueAsync(AnonymousJobArgs.JobNameConstant, anonymousArgs, priority, delay);
        }

        var job = Options.GetJob(jobName);
        return await EnqueueAsync(job, args, priority, delay);
    }

    protected virtual bool ShouldWrapAsAnonymousJob(string jobName)
    {
        return jobName != AnonymousJobArgs.JobNameConstant && AnonymousJobHandlerRegistry.IsRegistered(jobName);
    }

    protected virtual async Task<string> EnqueueAsync(BackgroundJobConfiguration job, object args, BackgroundJobPriority priority, TimeSpan? delay)
    {
        var timeTicker = new TimeTickerEntity
        {
            Id = Guid.NewGuid(),
            Function = job.JobName,
            ExecutionTime = delay == null ? DateTime.UtcNow : DateTime.UtcNow.Add(delay.Value),
            Request = CreateTickerRequest(job.ArgsType, args),
        };

        var config = TickerQOptions.GetConfigurationOrNull(job.JobType);
        if (config != null)
        {
            timeTicker.Retries = config.Retries ?? timeTicker.Retries;
            timeTicker.RetryIntervals = config.RetryIntervals ?? timeTicker.RetryIntervals;
            timeTicker.RunCondition = config.RunCondition ?? timeTicker.RunCondition;
        }

        var result = await TimeTickerManager.AddAsync(timeTicker);
        return !result.IsSucceeded ? timeTicker.Id.ToString() : result.Result.Id.ToString();
    }

    protected virtual byte[]? CreateTickerRequest(Type argsType, object args)
    {
        return (byte[]?)CreateTickerRequestMethod
            .MakeGenericMethod(argsType)
            .Invoke(null, [args]);
    }
}
