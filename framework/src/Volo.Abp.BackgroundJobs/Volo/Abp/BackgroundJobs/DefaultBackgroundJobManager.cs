using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Timing;

namespace Volo.Abp.BackgroundJobs;

/// <summary>
/// Default implementation of <see cref="IBackgroundJobManager"/>.
/// </summary>
[Dependency(ReplaceServices = true)]
public class DefaultBackgroundJobManager : IBackgroundJobManager, ITransientDependency
{
    protected IClock Clock { get; }
    protected IBackgroundJobSerializer Serializer { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IBackgroundJobStore Store { get; }
    protected IAnonymousJobHandlerRegistry AnonymousJobHandlerRegistry { get; }
    protected IOptions<AbpBackgroundJobOptions> BackgroundJobOptions { get; }
    protected IOptions<AbpBackgroundJobWorkerOptions> BackgroundJobWorkerOptions { get; }
    public ILogger<DefaultBackgroundJobManager> Logger { get; set; }

    public DefaultBackgroundJobManager(
        IClock clock,
        IBackgroundJobSerializer serializer,
        IBackgroundJobStore store,
        IGuidGenerator guidGenerator,
        IAnonymousJobHandlerRegistry anonymousJobHandlerRegistry,
        IOptions<AbpBackgroundJobOptions> backgroundJobOptions,
        IOptions<AbpBackgroundJobWorkerOptions> backgroundJobWorkerOptions)
    {
        Clock = clock;
        Serializer = serializer;
        GuidGenerator = guidGenerator;
        AnonymousJobHandlerRegistry = anonymousJobHandlerRegistry;
        BackgroundJobOptions = backgroundJobOptions;
        BackgroundJobWorkerOptions = backgroundJobWorkerOptions;
        Store = store;
        Logger = NullLogger<DefaultBackgroundJobManager>.Instance;
    }

    public virtual async Task<string> EnqueueAsync<TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal, TimeSpan? delay = null)
    {
        var jobName = BackgroundJobOptions.Value.GetBackgroundJobName(typeof(TArgs));
        return await EnqueueAsync(jobName, args!, priority, delay);
    }

    public virtual async Task<string> EnqueueAsync(string jobName, object args, BackgroundJobPriority priority = BackgroundJobPriority.Normal, TimeSpan? delay = null)
    {
        if (ShouldWrapAsAnonymousJob(jobName))
        {
            Logger.LogInformation(
                "Wrapping job into anonymous transport. TransportJobName: {TransportJobName}, EffectiveJobName: {EffectiveJobName}",
                AnonymousJobArgs.JobNameConstant,
                jobName
            );
            var jsonData = Serializer.Serialize(args);
            var anonymousArgs = new AnonymousJobArgs(jobName, jsonData);
            return await EnqueueAsync(AnonymousJobArgs.JobNameConstant, anonymousArgs, priority, delay);
        }

        var jobInfo = new BackgroundJobInfo
        {
            Id = GuidGenerator.Create(),
            ApplicationName = BackgroundJobWorkerOptions.Value.ApplicationName,
            JobName = jobName,
            JobArgs = Serializer.Serialize(args),
            Priority = priority,
            CreationTime = Clock.Now,
            NextTryTime = Clock.Now
        };

        if (delay.HasValue)
        {
            jobInfo.NextTryTime = Clock.Now.Add(delay.Value);
        }

        await Store.InsertAsync(jobInfo);

        return jobInfo.Id.ToString();
    }

    protected virtual bool ShouldWrapAsAnonymousJob(string jobName)
    {
        return jobName != AnonymousJobArgs.JobNameConstant &&
               AnonymousJobHandlerRegistry.IsRegistered(jobName) &&
               BackgroundJobOptions.Value.GetJobOrNull(jobName) == null;
    }
}
