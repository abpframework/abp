using System;
using System.Threading.Tasks;
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
    
    public DefaultBackgroundJobManager(
        IClock clock,
        IBackgroundJobSerializer serializer,
        IBackgroundJobStore store,
        IGuidGenerator guidGenerator)
    {
        Clock = clock;
        Serializer = serializer;
        GuidGenerator = guidGenerator;
        Store = store;
    }

    public virtual async Task<string> EnqueueAsync<TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal, TimeSpan? delay = null)
    {
        var jobName = BackgroundJobNameAttribute.GetName<TArgs>();
        
        var executionTime = delay.HasValue ? Clock.Now.Add(delay.Value) : (DateTime?) null;
        
        var jobId = await EnqueueAsync(jobName, args, priority, executionTime);
        
        return jobId.ToString();
    }

    public virtual async Task<string> EnqueueAsync<TArgs>(TArgs args, DateTime executionTime, BackgroundJobPriority priority = BackgroundJobPriority.Normal)
    {
        var jobName = BackgroundJobNameAttribute.GetName<TArgs>();
        
        var jobId = await EnqueueAsync(jobName, args, priority, executionTime);
        
        return jobId.ToString();
    }

    protected virtual async Task<Guid> EnqueueAsync(string jobName, object args, BackgroundJobPriority priority = BackgroundJobPriority.Normal, DateTime? executionTime = null)
    {
        var jobInfo = new BackgroundJobInfo
        {
            Id = GuidGenerator.Create(),
            JobName = jobName,
            JobArgs = Serializer.Serialize(args),
            Priority = priority,
            CreationTime = Clock.Now,
            NextTryTime = Clock.Now
        };

        if (executionTime.HasValue)
        {
            jobInfo.NextTryTime = executionTime.Value;
        }

        await Store.InsertAsync(jobInfo);

        return jobInfo.Id;
    }
}